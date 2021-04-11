using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HtmlAgilityPack;
using SuperMemoAssistant.Services;
using SuperMemoAssistant.Services.IO.HotKeys;
using SuperMemoAssistant.Services.IO.Keyboard;
using SuperMemoAssistant.Services.Sentry;
using SuperMemoAssistant.Services.UI.Configuration;
using SuperMemoAssistant.Sys.IO.Devices;

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
    public TextFormattingCfg Config { get; set; }

    #endregion

    #region Methods Impl

    private async Task LoadConfig()
    {
      Config = await Svc.Configuration.Load<TextFormattingCfg>().ConfigureAwait(false) ?? new TextFormattingCfg();
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
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeAlphanumeric),
        ToggleSuperscript
      )

     // SUBSCRIPT
     .RegisterGlobal(
        "FormatTextSubscript",
        "Subscript",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeCodeInput),
        ToggleSubscript
     )

     // STRIKETHROUGH
     .RegisterGlobal(
        "FormatTextStrikethrough",
        "Strikethrough",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeDbcsChar),
        ToggleStrikethrough
     )

     // JUSTIFY CENTER
     .RegisterGlobal(
        "FormatTextJustifyCenter",
        "Justify Center",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeEnterDialogConversionMode),
        JustifyCenter
     )

     // INDENT
     .RegisterGlobal(
        "FormatTextIndent",
        "Indent",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeEnterImeConfigureMode),
        Indent
     )

     // OUTDENT
     .RegisterGlobal(
        "FormatTextOutdent",
        "Outdent",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeEnterWordRegisterMode),
        Outdent
     )

     // INSERT LINE
     .RegisterGlobal(
        "FormatTextInsertLine",
        "Insert Horizontal Line",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeFlushString),
        InsertHorizontalRule
     )

     // JUSTIFY RIGHT
     .RegisterGlobal(
        "FormatTextJustifyRight",
        "Justify Right",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeHiragana),
        JustifyRight
     )

     // JUSTIFY LEFT
     .RegisterGlobal(
        "FormatTextJustifyLeft",
        "Justify Left",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeKatakana),
        JustifyLeft
     )

     // JUSTIFY FULL
     .RegisterGlobal(
        "FormatTextJustifyFull",
        "Justify Full",
        HotKeyScope.SMBrowser,
        new HotKey(Key.DbeNoCodeInput),
        JustifyFull
      )

     // Paste Plain Text
     .RegisterGlobal(
        "PastePlainText",
        "Paste Plain Text",
        HotKeyScope.SMBrowser,
        new HotKey(Key.V, KeyModifiers.CtrlShift),
        PastePlainText
     )

     .RegisterGlobal(
        "IncreseFontSize",
        "Increase Selected Text Font Size",
        HotKeyScope.SMBrowser,
        new HotKey(Key.OemPlus, KeyModifiers.CtrlAltShift),
        IncreaseFontSizeOfSelected
        )

     .RegisterGlobal(
        "DecreaseFontSize",
        "Decrease Selected Text Font Size",
        HotKeyScope.SMBrowser,
        new HotKey(Key.OemMinus, KeyModifiers.CtrlAltShift),
        DecreaseFontSizeOfSelected
        );
    }

    /// <inheritdoc />
    protected override void PluginInit()
    {
      LoadConfig().Wait();
      RegisterDummyHotkeys();
    }

    private void Outdent() => Commands.ExecuteCommand(HtmlCommand.Outdent, null);
    private void Indent() => Commands.ExecuteCommand(HtmlCommand.Indent, null);
    private void JustifyCenter() => Commands.ExecuteCommand(HtmlCommand.JustifyCenter, null);
    private void ToggleSuperscript() => Commands.ExecuteCommand(HtmlCommand.Superscript, null);
    private void ToggleSubscript() => Commands.ExecuteCommand(HtmlCommand.Subscript, null);
    private void ToggleStrikethrough() => Commands.ExecuteCommand(HtmlCommand.StrikeThrough, null);
    private void InsertHorizontalRule() => Commands.ExecuteCommand(HtmlCommand.InsertHorizontalRule, null);
    private void JustifyRight() => Commands.ExecuteCommand(HtmlCommand.JustifyRight, null);
    private void JustifyLeft() => Commands.ExecuteCommand(HtmlCommand.JustifyRight, null);
    private void JustifyFull() => Commands.ExecuteCommand(HtmlCommand.JustifyRight, null);

    private void PastePlainText()
    {
      Application.Current.Dispatcher.Invoke(() =>
      {
        string text = Clipboard.GetText();
        string plain = CleanHtml(text);
        if (!string.IsNullOrEmpty(plain))
        {
          Clipboard.SetData(DataFormats.Text, plain);
          Commands.ExecuteCommand(HtmlCommand.Paste, null);
        }
      });
    }

    private void IncreaseFontSizeOfSelected()
    {
      var size = Commands.ExecuteQuery(HtmlCommand.FontSize);
      if (size is int number)
        Commands.ExecuteCommand(HtmlCommand.FontSize, number + Config.FontSizeChangeInterval);
    }

    private void DecreaseFontSizeOfSelected()
    {
      var size = Commands.ExecuteQuery(HtmlCommand.FontSize);
      if (size is int number && number > 1)
        Commands.ExecuteCommand(HtmlCommand.FontSize, number - Config.FontSizeChangeInterval);
    }

    private string CleanHtml(string text)
    {
      var doc = new HtmlDocument();
      doc.LoadHtml(text);
      return doc.DocumentNode.InnerText;
    }

    #endregion

    #region Methods
    #endregion
  }
}
