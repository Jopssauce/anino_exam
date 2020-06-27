using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    public GameObject symbolPrefab;
    public GameObject reelContent;
    public GameObject targetSymbol;

    public float maxHeight = 0.5f;
    public float minHeight = -1f;

    public List<GameObject> children = new List<GameObject>();

    public bool stop = true;
    bool spin = true;
    public bool isSpinning = false;

    public void Initialize(int rows, List<SymbolData> symbols)
    {
        for (int i = 0; i < rows; i++)
        {
            GameObject symbolObject = Instantiate(symbolPrefab, reelContent.transform.position, reelContent.transform.rotation, reelContent.transform);
        }

        for (int i = 0; i < reelContent.transform.childCount; i++)
        {
            children.Add(reelContent.transform.GetChild(i).gameObject);
        }

        children[0].transform.position = new Vector3(transform.position.x, 0.5f, 0);
        for (int i = 0; i < children.Count; i++)
        {
            if (i + 1 < children.Count)
            {
                children[i + 1].transform.position = children[i].transform.position - new Vector3(0, 0.5f, 0);
            }
        }
        children[children.Count - 1].transform.position = new Vector3(transform.position.x, 1f, 0);

        SetSymbols(symbols.ToArray());

        targetSymbol = children[0];
    }

    public void SetSymbols(SymbolData[] symbols)
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].GetComponent<Symbol>().Initialize(symbols[i]);
        }
    }

    public void ResetReel()
    {
        for (int i = 0; i < children.Count; i++)
        {
            Destroy(children[i].gameObject);
        }
        children.Clear();
    }

    private void Start()
    {
        //Initialize(3, symbols);
    }

    private void FixedUpdate()
    {
        if(spin) Spin();
    }

    public void Spin()
    {
        GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (!stop)
            {
                children[i].transform.position += Vector3.down * 4f * Time.deltaTime;
                isSpinning = true;
            }
            else
            {
                if (children[0].GetComponent<Symbol>().symbolData.GetId() == targetSymbol.GetComponent<Symbol>().symbolData.GetId())
                {
                    isSpinning = false;
                    return;
                }
                children[i].transform.position += Vector3.down * 4f * Time.deltaTime;
            }

            if (children[i].transform.position.y < minHeight)
            {
                children[i].transform.position = new Vector3(children[i].transform.position.x, maxHeight, 0);
            }

            if (children[i].transform.position.y >= 0.5f && children[i].transform.position.y <= 0.6f)
            {
                children[i].transform.SetAsFirstSibling();
            }

        }
    }

    public void GetChildren()
    {
        children.Clear();
        for (int i = 0; i < reelContent.transform.childCount; i++)
        {
            children.Add(reelContent.transform.GetChild(i).gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 1, 0), new Vector3(transform.position.x, transform.position.y - 1, 0));
    }
}
