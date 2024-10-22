using UnityEngine;
using ImGuiNET;
using System.Collections.Generic;

public class ImGuiGraphic
{
    private float[] values_;
    public float[] Values
    {
        get { return values_; }
    }
    private int valueCount_;


    public ImGuiGraphic(uint count)
    {
        values_ = new float[count];
        valueCount_ = 0;
    }

    public void BeginHistogram(string name, float min, float max)
    {
        if (valueCount_ > 0) ImGui.PlotHistogram(name, ref values_[0], valueCount_, 0, "", min, max);
    }

    public void BeginLines(string name, float min, float max)
    {
        if (valueCount_ > 1) ImGui.PlotLines(name, ref values_[0], valueCount_, 0, "", min, max);
    }

    public void AddValue(float value)
    {
        if(valueCount_ < values_.Length)
        {
            values_[valueCount_] = value;
            valueCount_++;
        }
        else
        {
            RearrangeValues();
            values_[valueCount_ - 1] = value;
        }
    }

    private void RearrangeValues()
    {
        for(int i = 0; i < values_.Length - 1; i++)
        {
            values_[i] = values_[i + 1];
        }
    }
    public void AddList(List<float> list)
    {
        values_ = new float[list.Count];
        valueCount_ = 0;
        for (int i = 0; i < list.Count; ++i)
        {
            values_[i] = list[i];
            valueCount_++;
        }
    }
}