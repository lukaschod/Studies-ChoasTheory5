using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
	public Iteration iteration;
	public Growth growth;
	public Tree tree;
	public Button iterationButton;
	public Button growthButton;
	public Button treeButton;

	public void OnIteration()
	{
		iteration.gameObject.SetActive(true);
		iterationButton.interactable = false;
		growth.gameObject.SetActive(false);
		growthButton.interactable = true;
		tree.gameObject.SetActive(false);
		treeButton.interactable = true;
	}

	public void OnGrowth()
	{
		iteration.gameObject.SetActive(false);
		iterationButton.interactable = true;
		growth.gameObject.SetActive(true);
		growthButton.interactable = false;
		tree.gameObject.SetActive(false);
		treeButton.interactable = true;
	}

	public void OnTree()
	{
		iteration.gameObject.SetActive(false);
		iterationButton.interactable = true;
		growth.gameObject.SetActive(false);
		growthButton.interactable = true;
		tree.gameObject.SetActive(true);
		treeButton.interactable = false;
	}

	private void Start()
	{
		OnIteration();
	}
}
