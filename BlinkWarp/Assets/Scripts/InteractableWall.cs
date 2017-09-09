using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWall : InteractableSurface {

    public override void onHover()
    {
        player.GetComponent<PlayerCharacter>().DrawBlinkRangeCircle(20);
        //player.GetComponent<PlayerCharacter>().DrawBlinkRangeSphere(20,4);

        if (player.GetComponent<PlayerCharacter>().InBlinkRange())
        {
            player.GetComponent<PlayerCharacter>().DrawLineToInteractionPoint(Color.white);
        }
        else
        {
            player.GetComponent<PlayerCharacter>().DrawLineToInteractionPoint(Color.red);
        }
    }

    public override void onClickDown()
    {
        if (player.GetComponent<PlayerCharacter>().InBlinkRange())
        {
            player.GetComponent<PlayerCharacter>().WarpToContact();
            player.GetComponent<PlayerCharacter>().StickPlayerTo(this);
        }
    }

    public override void onClickHold() { }
    public override void onClickUp() { }

    public override void onKeyDown() { }
    public override void onKeyHold() { }
    public override void onKeyUp() { }
}
