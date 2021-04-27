using mshtml;
using SuperMemoAssistant.Extensions;
using SuperMemoAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.TextFormatting
{
  public static class ContentUtils
  {

    /// <summary>
    /// Get the IHTMLDocument2 representing the currently focused control.
    /// </summary>
    /// <returns>IHTMLDocument2 or null</returns>
    public static IHTMLDocument2 GetFocusedHtmlDoc()
    {
      try
      {
        var ctrlGroup = Svc.SM.UI.ElementWdw.ControlGroup;
        var htmlCtrl = ctrlGroup?.FocusedControl?.AsHtml();
        return htmlCtrl?.GetDocument();
      }
      catch (RemotingException) { }
      catch (UnauthorizedAccessException) { }

      return null;
    }
  }
}
