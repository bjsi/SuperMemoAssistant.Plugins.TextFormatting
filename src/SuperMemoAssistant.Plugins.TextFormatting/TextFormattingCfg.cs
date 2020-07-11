using Forge.Forms.Annotations;
using Newtonsoft.Json;
using SuperMemoAssistant.Services.UI.Configuration;
using SuperMemoAssistant.Sys.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.TextFormatting
{
  [Form(Mode = DefaultFields.None)]
  [Title("Dictionary Settings",
        IsVisible = "{Env DialogHostContext}")]
  [DialogAction("cancel",
        "Cancel",
        IsCancel = true)]
  [DialogAction("save",
        "Save",
        IsDefault = true,
        Validates = true)]
  public class TextFormattingCfg : CfgBase<TextFormattingCfg>, INotifyPropertyChangedEx
  {

    [Field(Name = "Add TextFormatting commands to Dev Context Menu?")]
    public bool AddToContextMenu { get; set; } = true;

    [JsonIgnore]
    public bool IsChanged { get; set; }

    public override string ToString()
    {
      return "Useful Snippets";
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
