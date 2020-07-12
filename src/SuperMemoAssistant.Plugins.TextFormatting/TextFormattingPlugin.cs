#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   7/4/2020 12:29:50 AM
// Modified By:  james

#endregion




namespace SuperMemoAssistant.Plugins.TextFormatting
{
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Runtime.Remoting;
  using System.Windows.Input;
  using Anotar.Serilog;
  using mshtml;
  using SuperMemoAssistant.Plugins.DevContextMenu.Interop;
  using SuperMemoAssistant.Services;
  using SuperMemoAssistant.Services.IO.HotKeys;
  using SuperMemoAssistant.Services.IO.Keyboard;
  using SuperMemoAssistant.Services.Sentry;
  using SuperMemoAssistant.Services.UI.Configuration;
  using SuperMemoAssistant.Sys.IO.Devices;
  using SuperMemoAssistant.Sys.Remoting;

  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
  public class TextFormattingPlugin : SentrySMAPluginBase<TextFormattingPlugin>
  {
    #region Constructors

    /// <inheritdoc />
    public TextFormattingPlugin() : base("Enter your Sentry.io api key (strongly recommended)") { }
    #endregion


    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "TextFormatting";

    /// <inheritdoc />
    public override bool HasSettings => true;
    public TextFormattingCfg Config;

    #endregion

    #region Methods Impl

    private void LoadConfig()
    {
      Config = Svc.Configuration.Load<TextFormattingCfg>() ?? new TextFormattingCfg();
    }

    public override void ShowSettings()
    {
      ConfigurationWindow.ShowAndActivate(HotKeyManager.Instance, Config);
    }

    private void RegisterDummyHotkeys()
    {

      // SUPERSCRIPT
      Svc.HotKeyManager
     .RegisterGlobal(
        "FormatTextSuperscript",
        "Superscript",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeAlphanumeric),
        ToggleSuperscript
      )

     // SUBSCRIPT
     .RegisterGlobal(
        "FormatTextSubscript",
        "Subscript",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeCodeInput),
        ToggleSubscript
     )
     
     // STRIKETHROUGH
     .RegisterGlobal(
        "FormatTextStrikethrough",
        "Strikethrough",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeDbcsChar),
        ToggleStrikethrough
     )

     // JUSTIFY CENTER
     .RegisterGlobal(
        "FormatTextJustifyCenter",
        "Justify Center",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeEnterDialogConversionMode),
        JustifyCenter
     )

     // INDENT
     .RegisterGlobal(
        "FormatTextIndent",
        "Indent",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeEnterImeConfigureMode),
        Indent
     )

     // OUTDENT
     .RegisterGlobal(
        "FormatTextOutdent",
        "Outdent",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeEnterWordRegisterMode),
        Outdent
     )

     // INSERT LINE
     .RegisterGlobal(
        "FormatTextInsertLine",
        "Insert Horizontal Line",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeFlushString),
        InsertHorizontalRule
     )

     // JUSTIFY RIGHT
     .RegisterGlobal(
        "FormatTextJustifyRight",
        "Justify Right",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeHiragana),
        JustifyRight
     )

     // JUSTIFY LEFT
     .RegisterGlobal(
        "FormatTextJustifyLeft",
        "Justify Left",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeKatakana),
        JustifyLeft
     )

     // JUSTIFY FULL
     .RegisterGlobal(
        "FormatTextJustifyFull",
        "Justify Full",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeNoCodeInput),
        JustifyRight
      );

    }

    private void AddOptionalServiceIntegrations()
    {

      var svc = GetService<IDevContextMenu>();
      if (svc.IsNull())
        return;

      // OUTDENT
      if (Config.AddOutdentMenuItem)
      {

        if (svc.AddMenuItem(Name, "Outdent", new ActionProxy(Outdent)))
          LogTo.Debug("Successfully added Outdent command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Outdent command to Dev Context Menu");

      }

      // INDENT
      if (Config.AddIndentMenuItem)
      {

        if (svc.AddMenuItem(Name, "Indent", new ActionProxy(Indent)))
          LogTo.Debug("Successfully added Indent command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Indent command to Dev Context Menu");

      }

      // JUSTIFY CENTER
      if (Config.AddJustifyCenterMenuItem)
      {

        if (svc.AddMenuItem(Name, "Justify Center", new ActionProxy(JustifyCenter)))
          LogTo.Debug("Successfully added Justify Center command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Justify Center command to Dev Context Menu");

      }

      // SUPERSCRIPT
      if (Config.AddSuperscriptMenuItem)
      {

        if (svc.AddMenuItem(Name, "Superscript", new ActionProxy(ToggleSuperscript)))
          LogTo.Debug("Successfully added Superscript command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Superscript command to Dev Context Menu");

      }

      // SUBSCRIPT
      if (Config.AddSubscriptMenuItem)
      {

        if (svc.AddMenuItem(Name, "Subscript", new ActionProxy(ToggleSubscript)))
          LogTo.Debug("Successfully added Subscript command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Subscript command to Dev Context Menu");

      }


      // STRIKETHROUGH
      if (Config.AddStrikethroughMenuItem)
      {

        if (svc.AddMenuItem(Name, "Strikethrough", new ActionProxy(ToggleStrikethrough)))
          LogTo.Debug("Successfully added Strikethrough command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Strikethrough command to Dev Context Menu");

      }
        

      // INSERT LINE
      if (Config.AddInsertLineMenuItem)
      {

        if (svc.AddMenuItem(Name, "Insert Line", new ActionProxy(InsertHorizontalRule)))
          LogTo.Debug("Successfully added Insert Line command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Insert Line command to Dev Context Menu");

      }

      // JUSTIFY RIGHT
      if (Config.AddJustifyRightMenuItem)
      {

        if (svc.AddMenuItem(Name, "Justify Right", new ActionProxy(JustifyRight)))
          LogTo.Debug("Successfully added Justify Right command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Justify Right command to Dev Context Menu");

      }
        

      // JUSTIFY LEFT
      if (Config.AddJustifyLeftMenuItem)
      {

        if (svc.AddMenuItem(Name, "Justify Left", new ActionProxy(JustifyLeft)))
          LogTo.Debug("Successfully added Justify Left command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Justify Left command to Dev Context Menu");

      }

      // JUSTIFY RIGHT
      if (Config.AddJustifyFullMenuItem)
      {

        if (svc.AddMenuItem(Name, "Justify Full",  new ActionProxy(JustifyFull)))
          LogTo.Debug("Successfully added Justify Full command to Dev Context Menu");
        else
          LogTo.Warning("Failed to add Justify Full command to Dev Context Menu");

      }
    }

    /// <inheritdoc />
    protected override void PluginInit()
    {

      LoadConfig();

      RegisterDummyHotkeys();

      AddOptionalServiceIntegrations();

    }

    private void Outdent() => Commands.Execute(HtmlCommand.Outdent, null);
    private void Indent() => Commands.Execute(HtmlCommand.Indent, null);
    private void JustifyCenter() => Commands.Execute(HtmlCommand.JustifyCenter, null);
    private void ToggleSuperscript() => Commands.Execute(HtmlCommand.Superscript, null);
    private void ToggleSubscript() => Commands.Execute(HtmlCommand.Subscript, null);
    private void ToggleStrikethrough() => Commands.Execute(HtmlCommand.StrikeThrough, null);
    private void InsertHorizontalRule() => Commands.Execute(HtmlCommand.InsertHorizontalRule, null);
    private void JustifyRight() => Commands.Execute(HtmlCommand.JustifyRight, null);
    private void JustifyLeft() => Commands.Execute(HtmlCommand.JustifyRight, null);
    private void JustifyFull() => Commands.Execute(HtmlCommand.JustifyRight, null);

    #endregion

    #region Methods
    #endregion
  }
}
