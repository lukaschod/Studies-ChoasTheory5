using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iteration : MonoBehaviour
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
			var last = new Vector3(0, graph.SampleFunction(0), 0);
			var count = 100;
			var step = 2f / count;
			for (int i = 1; i < count; i++)
			{
				var x = i * step;
				var current = new Vector3(x, graph.SampleFunction(x), 0);
				renderer.AddLine(new Line(last, current, Color.blue));
				last = current;
			}
		}

		{
			var renderer = graph.line2Renderer;
			var last = new Vector3(0, graph.LineFunction(0), 0);
			var count = 100;
			var step = 2f / count;
			for (int i = 1; i < count; i++)
			{
				var x = i * step;
				var current = new Vector3(x, graph.LineFunction(x), 0);
				renderer.AddLine(new Line(last, current, Color.green));
				last = current;
			}
		}

		{
			var renderer = graph.line3Renderer;

			var last = new Vector3(graph.x, 0, 0);

			for (int i = 0; i < graph.iterations; i++)
			{
				var y = graph.SampleFunction(last.x);
				var current = new Vector3(last.x, y, 0);
				renderer.AddLine(new Line(last, current, Color.red));
				renderer.AddLine(new Line(current, new Vector3(y, y, 0), Color.red));
				last = new Vector3(y, y, 0);
			}
		}

		graph.bounds.size = new Vector2(1, 1);
		graph.RecreateGraphRenderer();

		dirty = false;
	}
}
