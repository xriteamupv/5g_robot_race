using UnityEngine;
using System.Collections.Generic;
using ImGuiNET;

public class ImGuiHeader
{
    public bool isVisible_ = true;

    public delegate void Clickable();
    public event Clickable onOpen_;
    public event Clickable onClose_;

    public void RemoveAllEvents()
    {
        onOpen_ = null;
        onClose_ = null;
    }

    public bool Begin(string name, ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None)
    {
        bool isOpen = ImGui.CollapsingHeader(name, flags);

        if(ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
        {
            if(isOpen)
            {
                if (onClose_ != null) onClose_();
            }
            else
            {
                if (onOpen_ != null) onOpen_();
            }
        }

        return isOpen;
    }
}
