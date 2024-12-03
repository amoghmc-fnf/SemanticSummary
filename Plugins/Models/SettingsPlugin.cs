using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Plugins.Models;

public class SettingsPlugin : ISettingsPlugin
{
    private Topic topic;
    private int promptLength;

    public SettingsPlugin()
    {
        topic = Topic.Generic;
        promptLength = 10;
    }

    [KernelFunction("get_topic")]
    [Description("Gets the topic name for the summary")]
    [return: Description("Topic name")]
    public Topic GetTopic()
    {
        return topic;
    }

    [KernelFunction("set_topic")]
    [Description("Sets the topic name for the summary")]
    public void SetTopic(Topic newTopic)
    {
        topic = newTopic;
    }

    [KernelFunction("get_length")]
    [Description("Gets the maximum word count for the LLM to output for the summary")]
    [return: Description("Output length")]
    public int GetPromptLength()
    {
        return promptLength;
    }

    [KernelFunction("set_length")]
    [Description("Sets the maximum word count for the LLM to output for the summary")]
    public void SetPromptLength(int newPromptLength)
    {
        promptLength = newPromptLength;
    }

    [KernelFunction("get_summary_prompt")]
    [Description("Gets the prompt for summary using the most recently updated settings for the topic name and prompt length")]
    public string GetSummaryPrompt()
    {
        return $"Summarize the above text for the topic {GetTopic()} " +
            $"in at most {GetPromptLength()} words. " +
            $"Given input can be anything and you need to frame it for {GetTopic()} only! ";
    }
}
