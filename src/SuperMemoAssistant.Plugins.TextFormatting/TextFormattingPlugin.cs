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

    /// <inheritdoc />
    protected override void PluginInit()
    {

      LoadConfig();

      Svc.HotKeyManager
     .RegisterGlobal(
        "FormatTextSuperscript",
        "Make the currently selected text superscript",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeAlphanumeric),
        ToggleSuperscript
      )
     .RegisterGlobal(
        "FormatTextSubscript",
        "Make the currently selected text subscript",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeCodeInput),
        ToggleSubscript
     )
     .RegisterGlobal(
        "FormatTextStrikethrough",
        "Make the currently selected text strikethrough",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeDbcsChar),
        ToggleStrikethrough
     )
     .RegisterGlobal(
        "FormatTextJustifyCenter",
        "Center justify the currently selected text",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeEnterDialogConversionMode),
        JustifyCenter
     )
     .RegisterGlobal(
        "FormatTextIndent",
        "Indent the currently selected text",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeEnterImeConfigureMode),
        Indent
     )
     .RegisterGlobal(
        "FormatTextOutdent",
        "Outdent the currently selected text",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeEnterWordRegisterMode),
        Outdent
     )
     .RegisterGlobal(
        "FormatTextInsertLine",
        "Insert a horizontal rule",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeFlushString),
        InsertHorizontalRule
     )
     .RegisterGlobal(
        "FormatTextJustifyRight",
        "Justify the currently selected text to the right",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeHiragana),
        JustifyRight
     )
     .RegisterGlobal(
        "FormatTextJustifyLeft",
        "Justify the currently selected text to the left",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeKatakana),
        JustifyLeft
     )
     .RegisterGlobal(
        "FormatTextJustifyFull",
        "Fully justify the currently selecte text",
        HotKeyScopes.SMBrowser,
        new HotKey(Key.DbeNoCodeInput),
        JustifyRight
      );

      if (!Config.AddToContextMenu)
        return;

      var svc = GetService<IDevContextMenu>();
      if (svc.IsNull())
        return;

      svc.AddMenuItem(Name, "Outdent",              new ActionProxy(Outdent));
      svc.AddMenuItem(Name, "Indent",               new ActionProxy(Indent));
      svc.AddMenuItem(Name, "Justify Center",       new ActionProxy(JustifyCenter));
      svc.AddMenuItem(Name, "Toggle Superscript",   new ActionProxy(ToggleSuperscript));
      svc.AddMenuItem(Name, "Toggle Subscript",     new ActionProxy(ToggleSubscript));
      svc.AddMenuItem(Name, "Toggle Strikethrough", new ActionProxy(ToggleStrikethrough));
      svc.AddMenuItem(Name, "Insert Line",          new ActionProxy(InsertHorizontalRule));
      svc.AddMenuItem(Name, "Justify Right",        new ActionProxy(JustifyRight));
      svc.AddMenuItem(Name, "Justify Left",         new ActionProxy(JustifyLeft));
      svc.AddMenuItem(Name, "Justify Full",         new ActionProxy(JustifyFull));

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
