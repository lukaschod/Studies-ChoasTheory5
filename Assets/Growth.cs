using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Growth : MonoBehaviour
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

		
		{
			var renderer = graph.line1Renderer;
			var last = new Vector3(0, graph.SampleFunction(graph.x), 0);
			for (int i = 1; i < graph.iterations; i++)
			{
				var current = new Vector3(i, graph.SampleFunction(last.y), 0);
				renderer.AddLine(new Line(last, current, Color.blue));
				last = current;
			}

		}

		graph.bounds.size = new Vector2(graph.iterations, 1);
		graph.RecreateGraphRenderer();

		dirty = false;
	}
}
