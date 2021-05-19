using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputController : MonoBehaviour
{

    [System.Serializable]
    public class SliderText
    {
        public Slider slider;
        public Text text;
    }

    public SliderText mapWidth;
    public SliderText mapHeight;
    public SliderText ItterationAmount;
    public SliderText wallsNeeded;
    public SliderText goldNeeded;
    public SliderText goldChance;
    public SliderText fillPercent;

    public Toggle autoGenToggle;
    public Toggle outsideWallsToggle;
    public Toggle newMapToggle;

    private void Awake()
    {
        WidthUpdate();
        HeightUpdate();
        ItterationsUpdate();
        AutoGenerate();
        OutsideWalls();
        WallsAmountUpdate();
        GoldWallsNeeded();
        GoldChance();
        FillChance();
    }

    public void WidthUpdate()
    {
        int nWidth = Mathf.RoundToInt(mapWidth.slider.value);

        mapWidth.text.text = "Map Width : " + nWidth;

        MapGenerator.Instance.mapSize.x = nWidth;

    }

    public void HeightUpdate()
    {
        int nHeight = Mathf.RoundToInt(mapHeight.slider.value);

        mapHeight.text.text = "Map Height : " + nHeight;

        MapGenerator.Instance.mapSize.y = nHeight;

    }

    public void ItterationsUpdate()
    {
        int nItterations = Mathf.RoundToInt(ItterationAmount.slider.value);

        ItterationAmount.text.text = "Itteration Amount : " + nItterations;

        MapGenerator.Instance.smoothItterations = nItterations;
    }

    public void AutoGenerate()
    {
        bool nGeneration = autoGenToggle.isOn;

        MapGenerator.Instance.autoGenerate = nGeneration;

        if(nGeneration)
        {
            ItterationAmount.text.gameObject.SetActive(true);
        }
        else
            ItterationAmount.text.gameObject.SetActive(false);
    }

    public void OutsideWalls()
    {
        MapGenerator.Instance.createOutsideWalls = outsideWallsToggle.isOn;
    }

    public void WallsAmountUpdate()
    {
        int nWalls = Mathf.RoundToInt(wallsNeeded.slider.value);

        wallsNeeded.text.text = "Walls Needed : " + nWalls;

        MapGenerator.Instance.wallsNeeded = nWalls;
    }

    public void NewMapUpdate()
    {
        MapGenerator.Instance.useNewMap = newMapToggle.isOn;
    }

    public void GoldWallsNeeded()
    {
        int nWallsNeeded = Mathf.RoundToInt(goldNeeded.slider.value);

        goldNeeded.text.text = "Walls Needed For Gold : " + nWallsNeeded;

        MapGenerator.Instance.wallsNeededG = nWallsNeeded;
    }

    public void GoldChance()
    {
        int nGoldChance = Mathf.RoundToInt(goldChance.slider.value);

        goldChance.text.text = "Gold Chance : " + nGoldChance;

        MapGenerator.Instance.goldChance = nGoldChance;
    }

    public void FillChance()
    {
        int nFillChance = Mathf.RoundToInt(fillPercent.slider.value);

        fillPercent.text.text = "Fill Percent : " + nFillChance;

        MapGenerator.Instance.fillPercent = nFillChance;
    }


}
