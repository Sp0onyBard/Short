using UnityEngine;
using System.Collections;

/*This was originally written as part of a larger program that would maintain the status of the 
player's relationship with NPCs, but was abandoned in favor of a Unity Asset Store component. An NPC
would belong to a group(faction), and player actions would be associated with pleasing or angering factions. 
NPCs also have passions, which player actions could appease or slight as well. NPC's relationship to player
would be quantified with attitude. At the time work on this stopped, attitude was a 10-point scale ranging from
-5 to 5, where -5 is great dislike of the player and 5 is great admiration of player. 
Factions would play into the NPC's attitude, but it is not reflected below.*/
public class NPC {
    /*currently, the only variable in use is the attitude variable. the only methods in use are befriend and slight */
    public int attitude;
    public ArrayList passions;
    public string name;
    public string faction;
    public static int limits = 5;

    /*Create empty NPC with no passions*/
    public NPC(string name, string faction)
    {
        attitude = 0;
        passions = new ArrayList();
        this.name = name;
        this.faction = faction;
    }

    /*Create NPC with defined passions*/
    public NPC(string name, string faction, string p1, string p2, string p3)
    {
        this.name = name;
        this.faction = faction;
        passions.Add(p1);
        passions.Add(p2);
        passions.Add(p3);
        attitude = 0;
    }

    /*decrease overall attitude*/
    public void slight()
    { if ((attitude - 1) > (-1* limits))
        {
            attitude = attitude - 2;
        }
    }

    /*increase overall attitude*/
    public void befriend()
    {if ((attitude + 1) < limits)
        { attitude = attitude + 2; }
    }

    /*does this NPC have the given passion?*/
    public bool hasPassion(string p)
    {
        return passions.Contains(p);
    }

    /*Increase attitude by set amount*/
    public void addAtt(int n)
    {
        attitude = attitude + n;
    }

    /*Decrease attitude by set amount*/
    public void subAtt(int n)
    {
        attitude = attitude - n;
    }

    /*check if NPC is associated with a faction*/
    public bool hasFaction()
    {
        return (faction != null);
    }
}

