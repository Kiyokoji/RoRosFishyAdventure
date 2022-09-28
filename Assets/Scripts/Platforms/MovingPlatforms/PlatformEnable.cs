using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformEnable : MonoBehaviour
{
    public void EnablePlatform()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    public void DisablePlatform()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Platform");
    }

    public void Completed()
    {

    }
}
