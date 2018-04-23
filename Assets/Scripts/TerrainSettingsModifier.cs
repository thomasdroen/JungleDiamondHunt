using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainSettingsModifier : MonoBehaviour
{

    private Terrain terrain;

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

    private void OnEnable()
    {
        SettingsMenu.OnChangeSettings += OnSettingsChanged;
    }

    private void OnDestroy()
    {
        SettingsMenu.OnChangeSettings -= OnSettingsChanged;
    }

    private void OnSettingsChanged(object obj, SettingsEventArgs args)
    {
        
        switch (args.QualityLevel)
        {
            case 0:
                terrain.drawTreesAndFoliage = false;
                break;

            case 1:
                terrain.drawTreesAndFoliage = true;
                terrain.detailObjectDistance = 15;
                terrain.detailObjectDensity = 0.2f;
                break;

            case 2:
                terrain.drawTreesAndFoliage = true;
                terrain.detailObjectDistance = 20;
                terrain.detailObjectDensity = 0.25f;
                break;

            case 3:
                terrain.drawTreesAndFoliage = true;
                terrain.detailObjectDistance = 20;
                terrain.detailObjectDensity = 0.5f;
                break;

            case 4:
                terrain.drawTreesAndFoliage = true;
                terrain.detailObjectDistance = 25;
                terrain.detailObjectDensity = 0.5f;
                break;

            case 5:
                terrain.drawTreesAndFoliage = true;
                terrain.detailObjectDistance = 25;
                terrain.detailObjectDensity = 0.75f;
                break;

            default:
                terrain.drawTreesAndFoliage = true;
                terrain.detailObjectDistance = 25;
                terrain.detailObjectDensity = 1;
                break;
        }
    }
}
