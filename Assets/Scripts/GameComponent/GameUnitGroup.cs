using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnitGroup : OnMapUnitDisplay, GameWMTileOccupant
{
    private UnitGroup group;
    private Animator groupAnimator;
    private GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        model = StaticData.findDeepChild(transform, "model").gameObject;
        groupAnimator = model.GetComponent<Animator>();
    }

    public void setGroup(UnitGroup group)
    {
        this.group = group;
    }
    public UnitGroup getGroup()
    {
        return group;
    }
    public override Unit getDisplayUnit()
    {
        return group.getLeader();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
