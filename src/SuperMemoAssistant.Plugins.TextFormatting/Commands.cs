using Anotar.Serilog;
using mshtml;
using SuperMemoAssistant.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;


// Loosely based on: https://github.com/aAmitSengar/WindowsEditor/blob/master/Eq2ImgWinForms/WinHtmlEditor/HtmlEditor.cs 

namespace SuperMemoAssistant.Plugins.TextFormatting
{

  public enum HtmlCommand
  {
    FontName,
    FontSize,
    Print,
    Copy,
    Paste,
    Cut,
    Delete,
    Undo,
    Redo,
    RemoveFormat,
    JustifyCenter,
    JustifyFull,
    JustifyLeft,
    JustifyRight,
    Underline,
    Italic,
    Bold,
    BackColor,
    ForeColor,
    StrikeThrough,
    CreateLink,
    Unlink,
    InsertHorizontalRule,
    InsertImage,
    Outdent,
    Indent,
    InsertUnorderedList,
    InsertOrderedList,
    Superscript,
    Subscript,
    SelectAll
  }

  public static class Commands
  {

    /// <summary>
    /// Executes the execCommand on the selected document.
    /// Only affects the currently selected range.
    /// </summary>
    public static void ExecuteCommand(HtmlCommand command, object data)
    {

      var htmlDoc = ContentUtils.GetFocusedHtmlDoc();
      if (!htmlDoc.IsNull())
        ExecuteCommandHtmlDoc(htmlDoc, command, data);

    }

    /// <summary>
    /// Executes the execCommand on the currently focused document.
    /// Only affects the currently selected range.
    /// </summary>
    [LogToErrorOnException]
    public static void ExecuteCommandHtmlDoc(IHTMLDocument2 htmlDoc, HtmlCommand command, object data)
    {

      try
      {

        if (htmlDoc.IsNull())
          return;

        // ensure command is valid and enabled
        if (!htmlDoc.queryCommandSupported(command.Name()))
          return;

        if (!htmlDoc.queryCommandEnabled(command.Name()))
          return;

        htmlDoc.execCommand(command.Name(), false, data);

      }
      catch (RemotingException) { }
      catch (UnauthorizedAccessException) { }

    }

    public static void ExecuteQuery(HtmlCommand command)
    {

      var htmlDoc = ContentUtils.GetFocusedHtmlDoc();
      if (!htmlDoc.IsNull())
        QueryValueHtmlDoc(htmlDoc, command, data);

    }


    [LogToErrorOnException]
    public static object QueryValueHtmlDoc(IHTMLDocument2 htmlDoc, HtmlCommand command) 
    { 
      try
      {

        if (htmlDoc.IsNull())
          return null;

        // ensure command is valid and enabled
        if (!htmlDoc.queryCommandSupported(command.Name()))
          return null;

        if (!htmlDoc.queryCommandEnabled(command.Name()))
          return null;

        return htmlDoc.queryCommandValue(command.Name());

      }
      catch (RemotingException) { }
      catch (UnauthorizedAccessException) { }

      return null;

    }

  }
}
