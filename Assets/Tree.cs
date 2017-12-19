using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
	public InputField rField;
	public InputField cField;
	public InputField iterationsField;
	public InputField xField;
	public Graph graph;
	private bool dirty = false;

	public void SetRField(string value)
	{
		float.TryParse(value, out graph.r);
		dirty = true;
	}

	public void SetCField(string value)
	{
		float.TryParse(value, out graph.c);
		dirty = true;
	}

	public void SetIterationsField(string value)
	{
		int.TryParse(value, out graph.iterations);
		dirty = true;
	}

	public void SetXField(string value)
	{
		float.TryParse(value, out graph.x);
		dirty = true;
	}

	private void OnEnable()
	{
		dirty = true;
		rField.text = graph.r.ToString();
		cField.text = graph.c.ToString();
		iterationsField.text = graph.iterations.ToString();
		xField.text = graph.x.ToString();
	}

	private void Update()
	{
		if (!dirty)
			return;

		graph.Clear();
		var step = 0.001f;
		var extent = 1;

		{
			var renderer = graph.line1Renderer;
			var count = extent / step;
			float x = 0;
			for (int i = 0; i < count; i++)
			{
				x += step;
				var y = graph.x;
				for (int j = 0; j < graph.iterations; j++)
				{
					y = graph.SampleFunction(y, x);
					var position = new Vector3(x, y);
					renderer.AddPoint(new Point(position, Color.HSVToRGB(x, 1, 1)));

					
				}
			}
		}

		graph.bounds.size = new Vector2(1, 1);
		graph.RecreateGraphRenderer();

		dirty = false;
	}
}
