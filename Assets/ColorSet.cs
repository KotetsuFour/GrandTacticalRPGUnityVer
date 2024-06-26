using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSet
{

	private string title;
	private List<Color> colors;

	public ColorSet(string title, Color[] colors)
	{
		this.title = title;
		this.colors = new List<Color>();
		if (colors != null)
		{
			for (int q = 0; q < colors.Length; q++)
			{
				this.colors.Add(colors[q]);
			}
		}
	}

	public string getTitle()
	{
		return title;
	}

	public List<Color> getColors()
	{
		return colors;
	}

	public int size()
	{
		return colors.Count;
	}

	public Color colorAtIndex(int idx)
	{
		return colors[idx];
	}

	public void addColor(Color c)
	{
		colors.Add(c);
	}

	public void addColors(Color[] c)
	{
		for (int q = 0; q < c.Length; q++)
		{
			colors.Add(c[q]);
		}
	}
	public void addColors(List<Color> c)
	{
		colors.AddRange(c);
	}
	public void addColors(ColorSet c)
	{
		addColors(c.getColors());
	}
}
