using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPathLine : MonoBehaviour
{
    [SerializeField]
    GameObject line;
    [SerializeField] Material linemat;
    public Color startcolor = Color.yellow;
    public Color endcolor = Color.green;

    public CannonControlScript ccs;

    // Start is called before the first frame update
    void Start()
    {
        if(ccs == null)
        {
            ccs = GetComponent<CannonControlScript>();
        }

        line = new GameObject("Line Path");
        line.AddComponent<LineRenderer>();
        line.transform.parent = this.transform;
        line.transform.position = Vector3.zero;
        line.GetComponent<LineRenderer>().material = linemat;
        line.GetComponent<LineRenderer>().startColor = startcolor;
        line.GetComponent<LineRenderer>().endColor = endcolor;
        HideLine();
        
    }

    void Update()
    {
        if(line.activeSelf)
        {
            UpdateLine();
        }
    }

    public void ToggleLine()
    {
        if(line.activeSelf)
        {
            HideLine();
        }
        else
        {
            ShowLine();
        }
    }

    public void HideLine()
    {
        line.SetActive(false);
    }
    public void ShowLine()
    {
        line.SetActive(true);
        UpdateLine();
    }
    public void UpdateLine(int segments = 20, float timestep = 0.25f)
    {

        List<Vector3> position = new List<Vector3>();

        Vector3 initial_pos = ccs.firepoint.position;

        position.Add(initial_pos);

        for (int step = 0; step < segments; step++)
        {
            float time = timestep * step;
            position.Add(PhysicsEquation(initial_pos, time));
        }

        DrawLine(position);


    }


    private void DrawLine(List<Vector3> positions)
    {
        line.GetComponent<LineRenderer>().positionCount = positions.Count;
        for (var i = 0; i < positions.Count; ++i)
        {
            line.GetComponent<LineRenderer>().SetPosition(i, positions[i]);
        }
    }

    Vector3 PhysicsEquation(Vector3 start, float time)
    {
        return start + ccs.firepoint.up * ccs.currentPower * time + 0.5f * Physics.gravity * time * time;
    }
}