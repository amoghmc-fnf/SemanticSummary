using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummaryPluginPoc;

public class SummaryPlugin
{
    private SettingsPlugin settings;

    public SummaryPlugin(SettingsPlugin settings)
    {
        this.settings = settings;
    }

    [KernelFunction("get_summary_prompt")]
    [Description("Gets the prompt for summary")]
    public string GetSummaryPrompt()
    {
        return $"Summarize the above text for the topic {settings.GetTopic()} " +
            $"in at most {settings.GetPromptLength()} words. " +
            $"Given input can be anything and you need to frame it for {settings.GetTopic()} only! " +
            $"Summarize the text without any excuses!";
    }

}
