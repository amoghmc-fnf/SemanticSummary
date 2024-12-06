using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Plugins.Models;

public class SettingsPlugin : ISettingsPlugin
{
    private Topic topic;
    private int promptLength;
    private IConfiguration configuration;

    public SettingsPlugin(IConfiguration configuration)
    {
        topic = Topic.Generic;
        promptLength = 10;
        this.configuration = configuration;
    }

    [KernelFunction("get_topic")]
    [Description("Gets the latest topic name for the summary")]
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
    [Description("Gets the latest maximum word count for the LLM to output for the summary")]
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
    [Description("Gets the prompt for summary using the latest updated settings for the topic name and prompt length")]
    public string GetSummaryPrompt()
    {
        string topicPath = Path.Combine(configuration["PromptsForTopics"], $"{GetTopic()}.txt");
        string topicDescription = File.ReadAllText(topicPath);
        return $"Summarize the above text for the topic {GetTopic()} " +
            $"in at most {GetPromptLength()} words.\n" +
            $"Use the following description for the topic:\n{topicDescription}";
    }
}
