using UnityEngine;
using ImGuiNET;

public class ImGuiText
{

    private string value_;
    public string Value
    {
        get { return value_; }
        set { value_ = value; }
    }
    public void Begin(string text, Vector2 size = default(Vector2))
    {
        value_ = text;
        ImGui.Text(text);
    }
}
