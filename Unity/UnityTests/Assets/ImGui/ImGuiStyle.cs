using UnityEngine;
using ImGuiNET;

public class ImGuiStyle
{
    enum ImGuiCol_
    {
        ImGuiCol_Text,
        ImGuiCol_TextDisabled,
        ImGuiCol_WindowBg,
        ImGuiCol_ChildBg,
        ImGuiCol_PopupBg,
        ImGuiCol_Border,
        ImGuiCol_BorderShadow,
        ImGuiCol_FrameBg,
        ImGuiCol_FrameBgHovered,
        ImGuiCol_FrameBgActive,
        ImGuiCol_TitleBg,
        ImGuiCol_TitleBgActive,
        ImGuiCol_TitleBgCollapsed,
        ImGuiCol_MenuBarBg,
        ImGuiCol_ScrollbarBg,
        ImGuiCol_ScrollbarGrab,
        ImGuiCol_ScrollbarGrabHovered,
        ImGuiCol_ScrollbarGrabActive,
        ImGuiCol_CheckMark,
        ImGuiCol_SliderGrab,
        ImGuiCol_SliderGrabActive,
        ImGuiCol_Button,
        ImGuiCol_ButtonHovered,
        ImGuiCol_ButtonActive,
        ImGuiCol_Header,
        ImGuiCol_HeaderHovered,
        ImGuiCol_HeaderActive,
        ImGuiCol_Separator,
        ImGuiCol_SeparatorHovered,
        ImGuiCol_SeparatorActive,
        ImGuiCol_ResizeGrip,
        ImGuiCol_ResizeGripHovered,
        ImGuiCol_ResizeGripActive,
        ImGuiCol_Tab,
        ImGuiCol_TabHovered,
        ImGuiCol_TabActive,
        ImGuiCol_TabUnfocused,
        ImGuiCol_TabUnfocusedActive,
        ImGuiCol_PlotLines,
        ImGuiCol_PlotLinesHovered,
        ImGuiCol_PlotHistogram,
        ImGuiCol_PlotHistogramHovered,
        ImGuiCol_TextSelectedBg,
        ImGuiCol_DragDropTarget,
        ImGuiCol_NavHighlight,
        ImGuiCol_NavWindowingHighlight,
        ImGuiCol_NavWindowingDimBg,
        ImGuiCol_ModalWindowDimBg,
        ImGuiCol_COUNT

    };

    public static void Init()
    {
        var style = ImGui.GetStyle();

        style.WindowPadding = new Vector2(15, 15);
        style.WindowRounding = 0.0f;
        style.FramePadding = new Vector2(5, 5);
        style.FrameRounding = 5.0f;
        style.FrameBorderSize = 1.0f;
        style.WindowBorderSize = 1.0f;
        style.ItemSpacing = new Vector2(12, 8);
        style.ItemInnerSpacing = new Vector2(8, 6);
        style.IndentSpacing = 25.0f;
        style.ScrollbarSize = 15.0f;
        style.ScrollbarRounding = 9.0f;
        style.GrabMinSize = 5.0f;
        style.GrabRounding = 3.0f;


        style.Colors[(int)ImGuiCol_.ImGuiCol_Text] = new Vector4(0.80f, 0.80f, 0.83f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_TextDisabled] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_WindowBg] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ChildBg] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_PopupBg] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_Border] = new Vector4(0.80f, 0.80f, 0.83f, 0.88f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_BorderShadow] = new Vector4(0.92f, 0.91f, 0.88f, 0.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_FrameBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_FrameBgHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_FrameBgActive] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_TitleBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_TitleBgActive] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_TitleBgCollapsed] = new Vector4(1.00f, 0.98f, 0.95f, 0.75f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_MenuBarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ScrollbarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ScrollbarGrab] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ScrollbarGrabHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ScrollbarGrabActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_CheckMark] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_SliderGrab] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_SliderGrabActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_Button] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ButtonHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ButtonActive] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_Header] = new Vector4(0.20f, 0.29f, 0.32f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_HeaderHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_HeaderActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ResizeGrip] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ResizeGripHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ResizeGripActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_PlotLines] = new Vector4(0.40f, 0.39f, 0.98f, 0.63f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_PlotLinesHovered] = new Vector4(0.25f, 1.00f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_PlotHistogram] = new Vector4(0.40f, 0.39f, 0.98f, 0.63f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_PlotHistogramHovered] = new Vector4(0.25f, 1.00f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_TextSelectedBg] = new Vector4(0.25f, 0.25f, 1.00f, 0.43f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_ModalWindowDimBg] = new Vector4(1.00f, 0.98f, 0.95f, 0.73f);
        style.Colors[(int)ImGuiCol_.ImGuiCol_DragDropTarget] = new Vector4(1.00f, 0.98f, 0.95f, 0.73f);
    }
}
