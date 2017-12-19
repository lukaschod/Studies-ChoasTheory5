using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
	public Vector3 start;
	public Vector3 end;
	public Color color;

	public Line(Vector3 start, Vector3 end)
	{
		this.start = start;
		this.end = end;
		this.color = Color.white;
	}

	public Line(Vector3 start, Vector3 end, Color color)
	{
		this.start = start;
		this.end = end;
		this.color = color;
	}
}

public struct Point
{
	public Vector3 position;
	public Color color;

	public Point(Vector3 position)
	{
		this.position = position;
		this.color = Color.white;
	}

	public Point(Vector3 position, Color color)
	{
		this.position = position;
		this.color = color;
	}
}

public struct Text
{
	public Rect rect;
	public Color color;
	public string content;

	public Text(Rect rect, string content)
	{
		this.rect = rect;
		this.content = content;
		color = Color.white;
	}
}

public class LinePointDrawer
{
	private List<Line> lines;
	private List<Point> points;
	private List<Text> texts;
	private Rect bounds;

	public Rect Bounds { get { return bounds; } }

	public LinePointDrawer()
	{
		lines = new List<Line>();
		points = new List<Point>();
		texts = new List<Text>();
	}

	public void AddLine(Line line)
	{
		lines.Add(line);
		CheckBounds(line.start);
		CheckBounds(line.end);
	}

	public void AddPoint(Point point)
	{
		points.Add(point);
	}

	public void AddText(Text text)
	{
		texts.Add(text);
	}

	private void CheckBounds(Vector3 point)
	{
		bounds.max = new Vector2(Mathf.Max(bounds.max.x, point.x), Mathf.Max(bounds.max.y, point.y));
		bounds.min = new Vector2(Mathf.Min(bounds.min.x, point.x), Mathf.Min(bounds.min.y, point.y));
	}

	public void DrawTexts(Vector3 translate, Vector3 scale)
	{
		var guiStyle = new GUIStyle();
		guiStyle.alignment = TextAnchor.MiddleCenter;

		/*foreach (var text in texts)
		{
			var rect = text.rect;
			var position = Dot(rect.position, scale) + new Vector2(translate.x, translate.y);
			rect.position = position;
			rect.size = Dot(rect.size, scale);
			rect.y = Screen.height - rect.y;

			guiStyle.normal.textColor = text.color;
			guiStyle.fontSize = (int)(Mathf.Min(rect.width, rect.height) * 0.7f);

			GUI.Label(rect, text.content, guiStyle);
		}*/

		foreach (var text in texts)
		{
			var rect = text.rect;
			var position = Dot(rect.center, scale) + new Vector2(translate.x, translate.y);
			rect.center = position;
			rect.size = Dot(rect.size, scale);
			rect.y = Screen.height - rect.y;

			guiStyle.normal.textColor = text.color;
			guiStyle.fontSize = (int)(Mathf.Min(rect.width, rect.height) * 0.35f);

			GUI.Label(rect, text.content, guiStyle);
		}
	}

	public Vector3 Dot(Vector3 first, Vector3 second)
	{
		return new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
	}

	public Vector2 Dot(Vector2 first, Vector2 second)
	{
		return new Vector2(first.x * second.x, first.y * second.y);
	}

	public Vector2 Dot(Vector2 first, Vector3 second)
	{
		return new Vector2(first.x * second.x, first.y * second.y);
	}

	public void DrawLines(Material lineMaterial, Vector3 translate, Vector3 scale)
	{
		// Draw batched lines
		GL.PushMatrix();
		GL.LoadPixelMatrix();
		GL.Begin(GL.LINES);
		lineMaterial.SetPass(0);
		foreach (var line in lines)
		{
			var start = Dot(line.start, scale) + translate;
			var end = Dot(line.end, scale) + translate;
			GL.Color(line.color);
			GL.Vertex(start);
			GL.Vertex(end);
		}
		GL.End();
		GL.PopMatrix();

		// Draw batched points
		GL.PushMatrix();
		GL.LoadPixelMatrix();
		GL.Begin(GL.LINES);
		lineMaterial.SetPass(0);
		foreach (var point in points)
		{
			var offset = new Vector3(1, 0, 0);
			var start = Dot(point.position, scale) + translate - offset;
			var end = Dot(point.position, scale) + translate + offset;
			GL.Color(point.color);
			GL.Vertex(start);
			GL.Vertex(end);
		}
		GL.End();
		GL.PopMatrix();
	}

	public void Clear()
	{
		lines.Clear();
		points.Clear();
		texts.Clear();
	}
}

public class Graph : MonoBehaviour
{
	public Material lineMaterial;
	public int gridXCount = 20;
	public int gridYCount = 20;
	public Vector3 offset;
	public Rect bounds = new Rect(0, 0, 2, 2);

	public float r = 2;
	public float c = 2;
	public float x = 1;
	public int iterations = 50;

	public LinePointDrawer graphRenderer;
	public LinePointDrawer line1Renderer;
	public LinePointDrawer line2Renderer;
	public LinePointDrawer line3Renderer;

	public float SampleFunction(float x)
	{
		return 4 * r * x * (1 - x);
		//return r * x * x * (1f - x) - c * x;
	}

	public float SampleFunction(float x, float r)
	{
		return 4 * r * x * (1 - x);
		//return r * x * x * (1f - x) - c * x;
	}

	public float LineFunction(float x)
	{
		return x;
	}

	public void Clear()
	{
		graphRenderer.Clear();
		line1Renderer.Clear();
		line2Renderer.Clear();
		line3Renderer.Clear();
	}

	public void RecreateGraphRenderer()
	{
		graphRenderer.Clear();

		// Add X axis
		graphRenderer.AddLine(new Line(Vector3.right * bounds.min.x, Vector3.right * bounds.max.x));

		// Add Y axis
		graphRenderer.AddLine(new Line(Vector3.up * bounds.min.y, Vector3.up * bounds.max.y));

		var gridSizeX = bounds.width / gridXCount;
		var gridSizeY = bounds.height / gridYCount;

		// Add grid on X axis
		var gridSize = gridSizeX;
		var gridSpikeSize = bounds.height / 50;
		var textSize = Mathf.Min(gridSize, bounds.width / 20);
		for (int i = 0; i < gridXCount; i++)
		{
			var offset = Vector3.right * gridSize * i;
			var number = i * gridSize;
			graphRenderer.AddLine(new Line(offset, offset + Vector3.up * gridSpikeSize));

			var rect = new Rect();
			rect.size = new Vector2(gridSize, gridSizeY);
			rect.center = offset + Vector3.up * gridSizeY + Vector3.left * gridSizeX/2 + Vector3.down * gridSizeY;
			graphRenderer.AddText(new Text(rect, number.ToString()));
		}

		// Add grid on Y axis
		gridSize = gridSizeY;
		gridSpikeSize = bounds.width / 50;
		textSize = Mathf.Min(gridSize, bounds.height / 20);
		for (int i = 0; i < gridYCount; i++)
		{
			var offset = Vector3.up * gridSize * i;
			var number = i * gridSize;
			graphRenderer.AddLine(new Line(offset, offset + Vector3.right * gridSpikeSize));

			var rect = new Rect();
			rect.size = new Vector2(gridSizeX, gridSizeY);
			rect.center = offset + Vector3.up * gridSizeY/2 + Vector3.left * gridSizeX;
			graphRenderer.AddText(new Text(rect, number.ToString()));
		}
	}

	private void Start()
	{
		graphRenderer = new LinePointDrawer();
		line1Renderer = new LinePointDrawer();
		line2Renderer = new LinePointDrawer();
		line3Renderer = new LinePointDrawer();
	}

	private void OnPostRender()
	{
		var scale = new Vector2(Screen.width / bounds.width, Screen.height / bounds.height) * 0.6f;
		
		line1Renderer.DrawLines(lineMaterial, offset, scale);
		line2Renderer.DrawLines(lineMaterial, offset, scale);
		line3Renderer.DrawLines(lineMaterial, offset, scale);
		graphRenderer.DrawLines(lineMaterial, offset, scale);
	}

	private void OnGUI()
	{
		var scale = new Vector2(Screen.width / bounds.width, Screen.height / bounds.height) * 0.6f;
		
		line1Renderer.DrawTexts(offset, scale);
		line2Renderer.DrawTexts(offset, scale);
		line3Renderer.DrawTexts(offset, scale);
		graphRenderer.DrawTexts(offset, scale);
	}
}
