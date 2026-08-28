using System.Text;

namespace Hail;

public static class ClaudeCodeAdapter
{
    public static string Compile(HailProfile profile)
    {
        var preferences = profile.Profile;
        var instructions = new StringBuilder();

        instructions.AppendLine("# HAIL Interaction Instructions");
        instructions.AppendLine();
        instructions.AppendLine("Adapt how you collaborate with this user using the following interaction preferences.");
        instructions.AppendLine();

        instructions.AppendLine(preferences.Verbosity switch
        {
            "compact" => "- Keep responses compact. Include necessary detail, but avoid expanding beyond what is needed to make progress.",
            "detailed" => "- Provide detailed responses when the task benefits from depth. Add useful explanatory layers such as reasoning, interactions, examples, tradeoffs, edge cases, or implementation implications rather than merely expanding wording. Remain proportionate to simple requests.",
            _ => "- Use balanced verbosity: enough detail to be useful without overwhelming the user."
        });

        instructions.AppendLine(preferences.DecisionMode switch
        {
            "options" => "- When helping with decisions, present the strongest options without forcing a recommendation unless one is clearly warranted.",
            "choose_by_default" => "- When the user is blocked by a reversible decision, choose a sensible default and proceed unless the choice carries material risk.",
            _ => "- When helping with decisions, give your recommended option first, then briefly explain why before presenting alternatives."
        });

        instructions.AppendLine($"- Present no more than {preferences.MaxOptions} options at once unless additional choices are necessary for correctness or safety.");

        instructions.AppendLine(preferences.TaskChunking switch
        {
            "off" => "- Keep the user's work whole by default rather than proactively decomposing it into execution steps. You may still use headings, bullets, or other presentation structure when useful for clarity or correctness, and break work into steps when the user explicitly asks.",
            "always" => "- For genuinely multi-step work, deliberately partition the work in the current response into small, concrete, independently actionable chunks. Do not merely describe or promise a future step-by-step plan, and do not manufacture a workflow for trivial or single-step requests.",
            _ => "- When a task is meaningfully complex, cognitively heavy, or easier to act on incrementally, decompose it into a small number of meaningful actionable chunks. Prefer broader chunks than an always-step-by-step approach, and answer simple requests directly without unnecessary decomposition."
        });

        instructions.AppendLine(preferences.StepPacing switch
        {
            "check_in" => "- Between meaningful steps, briefly check whether the user is ready to continue when doing so would reduce cognitive load.",
            "wait_for_user" => "- For multi-step work, give the user one small actionable step at a time. Stop after that step and wait for the user to explicitly indicate they are ready before giving or executing the next step.",
            _ => "- When presenting or executing steps, continue naturally unless the user asks you to pause."
        });

        instructions.AppendLine(preferences.TangentPolicy switch
        {
            "follow" => "- It is acceptable to follow conversational tangents when they appear useful to the user.",
            "redirect" => "- When a tangent appears, briefly acknowledge it and redirect attention to the active goal unless the user explicitly changes goals.",
            _ => "- When the user introduces a tangent during an active task, acknowledge or capture it without losing the original goal. Do not expand the tangent unless the user deliberately switches tasks."
        });

        return instructions.ToString().TrimEnd();
    }
}
