using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateToSimpleTextureCube : MonoBehaviour
{
    public SetTextureAsGrid m_grid;

    public RubikFaceDictionary<ShortVector2D> m_faceTo2DPosition = new RubikFaceDictionary<ShortVector2D>();

    public void Start()
    {
        DefinePosition();
    }
    
    public void RefreshWith(RubikCubeSaveState cubeState)
    {
        DoOnEachFace(SetColorOfEachFace, cubeState.m_givenCubeState);
    }

    public void SetColorOfEachFace(RubikCubeFace targetFace, RubikCubeFaceDirection targetDirection, RubikCubeFace currentFace, RubikCubeFaceDirection currentDirection) {
       
        Color color = RubikCube.GetColor(RubikCube.GetDefaultColor(currentFace));
        ShortVector2D pos = m_faceTo2DPosition.Get(targetFace, targetDirection);
        m_grid.SetColor(color, pos.x, pos.y);

    }

    public void DoOnEachFace(DoTheThingToAnyFace toDo)
    {
        foreach (RubikCubeFace f in Enum.GetValues(typeof(RubikCubeFace)).Cast<RubikCubeFace>().ToList())
        {
            foreach (RubikCubeFaceDirection d in Enum.GetValues(typeof(RubikCubeFaceDirection)).Cast<RubikCubeFaceDirection>().ToList())
            {
                toDo(f, d);
            }
        }
    }
    public void DoOnEachFace(DoTheThingToAnyRealFace toDo, CubeDirectionalState cubeState)
    {
        if (cubeState == null)
            return;
        foreach (RubikCubeFace f in Enum.GetValues(typeof(RubikCubeFace)).Cast<RubikCubeFace>().ToList())
        {
            foreach (RubikCubeFaceDirection d in Enum.GetValues(typeof(RubikCubeFaceDirection)).Cast<RubikCubeFaceDirection>().ToList())
            {
                RubikCubeFace currentFace;
                RubikCubeFaceDirection currentDirection;
                cubeState.GetRealPieceFaceInfoAt(f, d, out currentFace, out currentDirection);
                toDo(f, d, currentFace, currentDirection);
            }
        }
    }




    void DefinePosition()
    {

        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.NO, new ShortVector2D(3, 11));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.N, new ShortVector2D(4, 11));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.NE, new ShortVector2D(5, 11));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.O, new ShortVector2D(3, 10));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.C, new ShortVector2D(4, 10));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.E, new ShortVector2D(5, 10));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.SO, new ShortVector2D(3, 9));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.S, new ShortVector2D(4, 9));
        m_faceTo2DPosition.Add(RubikCubeFace.Up, RubikCubeFaceDirection.SE, new ShortVector2D(5, 9));

        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.NO, new ShortVector2D(3, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.N, new ShortVector2D(4, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.NE, new ShortVector2D(5, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.O, new ShortVector2D(3, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.C, new ShortVector2D(4, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.E, new ShortVector2D(5, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.SO, new ShortVector2D(3, 6));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.S, new ShortVector2D(4, 6));
        m_faceTo2DPosition.Add(RubikCubeFace.Face, RubikCubeFaceDirection.SE, new ShortVector2D(5, 6));



        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.NO, new ShortVector2D(3, 5));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.N, new ShortVector2D(4, 5));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.NE, new ShortVector2D(5, 5));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.O, new ShortVector2D(3, 4));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.C, new ShortVector2D(4, 4));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.E, new ShortVector2D(5, 4));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.SO, new ShortVector2D(3, 3));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.S, new ShortVector2D(4, 3));
        m_faceTo2DPosition.Add(RubikCubeFace.Down, RubikCubeFaceDirection.SE, new ShortVector2D(5, 3));


        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.NO, new ShortVector2D(3, 2));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.N, new ShortVector2D(4, 2));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.NE, new ShortVector2D(5, 2));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.O, new ShortVector2D(3, 1));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.C, new ShortVector2D(4, 1));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.E, new ShortVector2D(5, 1));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.SO, new ShortVector2D(3, 0));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.S, new ShortVector2D(4, 0));
        m_faceTo2DPosition.Add(RubikCubeFace.Back, RubikCubeFaceDirection.SE, new ShortVector2D(5, 0));

        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.NO, new ShortVector2D(0, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.N, new ShortVector2D(1, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.NE, new ShortVector2D(2, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.O, new ShortVector2D(0, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.C, new ShortVector2D(1, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.E, new ShortVector2D(2, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.SO, new ShortVector2D(0, 6));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.S, new ShortVector2D(1, 6));
        m_faceTo2DPosition.Add(RubikCubeFace.Left, RubikCubeFaceDirection.SE, new ShortVector2D(2, 6));

        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.NO, new ShortVector2D(6, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.N, new ShortVector2D(7, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.NE, new ShortVector2D(8, 8));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.O, new ShortVector2D(6, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.C, new ShortVector2D(7, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.E, new ShortVector2D(8, 7));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.SO, new ShortVector2D(6, 6));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.S, new ShortVector2D(7, 6));
        m_faceTo2DPosition.Add(RubikCubeFace.Right, RubikCubeFaceDirection.SE, new ShortVector2D(8, 6));

    }


}
public delegate void DoTheThingToAnyFace(RubikCubeFace targetFace, RubikCubeFaceDirection targetDirection);
public delegate void DoTheThingToAnyRealFace(RubikCubeFace targetFace, RubikCubeFaceDirection targetDirection, RubikCubeFace currentFace, RubikCubeFaceDirection currentDirection);
public class ShortVector2D
{
    public ShortVector2D(short x, short y)
    {
        this.x = x;
        this.y = y;
    }
    public short x;
    public short y;
}