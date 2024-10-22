using UnityEngine;
using ImGuiNET;

public class ImGuiWindow
{

    public static ImGuiWindowFlags genericFlags_ = ImGuiWindowFlags.NoMove
            | ImGuiWindowFlags.NoCollapse
            | ImGuiWindowFlags.NoResize;

    public static ImGuiWindowFlags genericdockingFlags_ = ImGuiWindowFlags.NoMove
            | ImGuiWindowFlags.NoCollapse
            | ImGuiWindowFlags.NoResize;


    public bool isOpen_ = true;

    public bool Begin(string name, ImGuiWindowFlags flags)
    {
        if(isOpen_)
        {
            return ImGui.Begin(name, flags);
        }
        return false;
    }

    public bool BeginClosable(string name, ImGuiWindowFlags flags)
    {
        if (isOpen_)
        {
            return ImGui.Begin(name, ref isOpen_, flags);
        }
        return false;
    }

    public void End()
    {
        ImGui.End();
    }

    public static void TextCentered(string text)
    {
        var windowWidth = ImGui.GetWindowSize().x;
        var textWidth = ImGui.CalcTextSize(text).x;

        ImGui.SetCursorPosX((windowWidth - textWidth) * 0.5f);
    }

    public static void SetWindowPosition(Vector2 pos)
    {
        ImGui.SetWindowPos(pos);
    }

    public static void SetWindowSize(Vector2 size)
    {
        ImGui.SetWindowSize(size);
    }

    public static Vector2 GetWindowSize()
    {
        return ImGui.GetWindowSize();
    }

    public static void CenterWindow()
    {
        Vector2 size = ImGui.GetWindowSize();
        Vector2 pos = new Vector2((Screen.width * 0.5f) - (size.x * 0.5f), (Screen.height * 0.5f) - (size.y * 0.5f));
        ImGui.SetWindowPos(pos);
    }

    public static void SameLine(float offset = 0.0f)
    {
        ImGui.SameLine(offset);
    }

    public static void NewLine()
    {
        ImGui.NewLine();
    }

    public static void Separator()
    {
        ImGui.Separator();
    }

    public static void VerticalSpacing()
    {
        ImGui.Spacing();
    }

    public static void VerticalSpacings(int count)
    {
        for(int i = 0; i < count; i++)
        {
            ImGui.Spacing();
        }
    }

    public static void BeginChild(string childName, Vector2 size = default(Vector2), bool border = false, ImGuiWindowFlags flags = ImGuiWindowFlags.None)
    {
        ImGui.BeginChild(childName, size, border, flags);
    }

    public static void EndChild()
    {
        ImGui.EndChild();
    }

    public static bool IsHoveringAny()
    {
        return ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow);
    }
}
