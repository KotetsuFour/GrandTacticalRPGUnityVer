using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : OnMapUnitDisplay, GameBFTileOccupant
{
    private Unit unit;
    private Animator unitAnimator;
    private GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        model = StaticData.findDeepChild(transform, "model").gameObject;
        unitAnimator = model.GetComponent<Animator>();
    }
    public void setUnit(Unit unit)
    {
        this.unit = unit;
    }
    public Unit getUnit()
    {
        return unit;
    }
    public override Unit getDisplayUnit()
    {
        return unit;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
