using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class ControllerDefault : Controller
{
    public string m_name="Default";
    public Transform m_position;
    public Transform m_direction;

    public override Vector3 GetFowardDirection()
    {
        return m_direction.forward;
    }

    public override string GetName()
    {
        return m_name;
    }

    public override Vector3 GetPosition(Space space = Space.World)
    {
        return space == Space.World ? m_position.position : m_position.localPosition;
    }

    public override Quaternion GetRotation(Space space = Space.World)
    {
        return space == Space.World ? m_position.rotation : m_position.localRotation;
    }

    public override bool IsActiveInHierarchy()
    {
       return  this.gameObject.activeInHierarchy;
    }

    public override bool IsPressing()
    {
        return Input.GetKey("Fire");
    }

    public void Reset()
    {
        if(m_position==null)
             m_position = this.transform;
        if (m_direction == null)
            m_direction = this.transform;
    }
}


public interface IController {
    Vector3 GetFowardDirection();
    Vector3 GetPosition(Space space = Space.World);
    Quaternion GetRotation(Space space = Space.World);
    bool IsActiveInHierarchy();
    bool IsPressing();
    string GetName();

}
public abstract class Controller : MonoBehaviour, IController
{
    public abstract Vector3 GetFowardDirection();

    public abstract Vector3 GetPosition(Space space = Space.World);

    public abstract Quaternion GetRotation(Space space = Space.World);
    public abstract bool IsActiveInHierarchy();

   
    public abstract string GetName();
    public abstract bool IsPressing();

    internal static Controller FindActiveOne(string word)
    {
        IEnumerable<Controller> controllers = FindObjectsOfType<Controller>().Where(k => k.IsActiveInHierarchy() && k.GetName().Contains(word));
        if (controllers.Count() > 0)
            return controllers.First();
        return null;
    }
    internal static Controller FindActiveOne(string [] keywords)
    {
        Controller controller=null;
        foreach (string name in keywords)
        {
            controller = FindActiveOne(name);
            if (controller != null)
                return controller;

        }
        return null;
    }
    public static Controller FindActiveOne()
    {

        return FindObjectsOfType<Controller>().Where(k => k.IsActiveInHierarchy()).First();
    }

}