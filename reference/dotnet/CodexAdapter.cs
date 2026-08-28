using System.Text;

namespace Hail;

public static class CodexAdapter
{
    public static string Compile(HailProfile profile)
    {
        var preferences = profile.Profile;
        var instructions = new StringBuilder();

        instructions.AppendLine("# HAIL Interaction Instructions");
        instructions.AppendLine();
        instructions.AppendLine("Apply these preferences when collaborating with this user. Treat them as interaction guidance, not project-specific engineering rules.");
        instructions.AppendLine();

        instructions.AppendLine(preferences.Verbosity switch
        {
            "compact" => "- Keep responses compact and action-oriented. Include only the detail needed to make safe, correct progress.",
            "detailed" => "- Provide detailed responses when the task benefits from depth. Add useful explanatory layers such as reasoning, interactions, examples, tradeoffs, edge cases, or implementation implications rather than merely expanding wording. Remain proportionate to simple requests.",
            _ => "- Use balanced verbosity: enough detail to support progress without unnecessary expansion."
        });

        instructions.AppendLine(preferences.DecisionMode switch
        {
            "options" => "- For decisions, present the strongest viable options without forcing a recommendation unless one is clearly warranted.",
            "choose_by_default" => "- For reversible, low-risk decisions, choose a sensible default and continue unless the choice materially affects safety, correctness, or user intent.",
            _ => "- For decisions, state your recommended option first, give a concise reason, then mention alternatives only as needed."
        });

        instructions.AppendLine($"- Present no more than {preferences.MaxOptions} options at once unless additional choices are necessary for correctness or safety.");

        instructions.AppendLine(preferences.TaskChunking switch
        {
            "off" => "- Do not automatically decompose work into smaller steps unless the user asks for that structure.",
            "always" => "- Break multi-step work into small, concrete, executable steps and make the next action obvious.",
            _ => "- When work is complex, ambiguous, or cognitively heavy, reduce it to a small number of concrete next steps. Avoid over-structuring simple work."
        });

        instructions.AppendLine(preferences.StepPacing switch
        {
            "check_in" => "- Between meaningful steps, briefly check whether the user is ready to continue when doing so would reduce cognitive load.",
            "wait_for_user" => "- For multi-step work, present one small actionable step at a time and wait for explicit user readiness before presenting or executing the next meaningful step.",
            _ => "- When presenting or executing steps, continue naturally unless the user asks you to pause."
        });

        instructions.AppendLine(preferences.TangentPolicy switch
        {
            "follow" => "- It is acceptable to follow a new conversational tangent when it appears useful to the user.",
            "redirect" => "- When a tangent appears during active work, acknowledge it briefly and redirect to the active goal unless the user explicitly changes goals.",
            _ => "- When the user introduces a tangent during active work, capture or acknowledge it without abandoning the original goal. Do not expand the tangent unless the user deliberately switches goals."
        });

        return instructions.ToString().TrimEnd();
    }
}
