using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2,256)]
    public int resolution;

    public bool autoUpdate = true; 

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool ShapeSettingsFoldout;
    [HideInInspector]
    public bool ColorSettingsFoldout;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;


    private void OnValidate()
    {
        GeneratePlanet();
    }
    void Initilize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        if(meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] direction = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        for (int i = 0; i < 6; i++)
        {

            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();

                meshFilters[i].sharedMesh = new Mesh();

            }
            terrainFaces[i] = new TerrainFace(shapeGenerator,meshFilters[i].sharedMesh, resolution, direction[i]);


        }
    }
    public void GeneratePlanet()
    {
        Initilize();
        GenerateMesh();
        GenerateColors();
    }
    public void OnShapeSettingUpdated()
    {
        if (autoUpdate)
        {
            Initilize();
            GenerateMesh();
        }
       
    }
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initilize();
            GenerateColors();
        }
    }

    void GenerateMesh()
    {
        foreach(TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
    void GenerateColors()
    {
        foreach (MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
        }
    }
}
