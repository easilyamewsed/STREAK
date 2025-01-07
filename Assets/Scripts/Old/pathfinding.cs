using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<Node> allNodes;
    public Node playerNode;

    public List<Node> foundPath;
    public Node startNodeTesting;

    PlayerMovement player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        List<GameObject> nodeObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Node"));
        foreach (var node in nodeObjects)
        {
            allNodes.Add(node.GetComponent<Node>());
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foundPath = FindPath(startNodeTesting);
        }
    }

    public Node FindPlayer()
    {
        float distanceToPlayer; 
        Node closestNode = allNodes[0];
        float shortestDistance = (allNodes[0].transform.position - player.transform.position).magnitude;

        foreach (Node node in allNodes)
        {
            distanceToPlayer = (node.transform.position - player.transform.position).magnitude;

            if (distanceToPlayer < shortestDistance)
            {
                closestNode = node;
                shortestDistance = distanceToPlayer; 
            }
        }
        
        return closestNode;
    }

    public List<Node> FindPath(Node startNode)
    {
        Node endNode = FindPlayer();
        List<Node> exploredNodes = new List<Node>();
        List<Node> unexploredNodes = new List<Node>();

        List<GameObject> nodeObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Node"));
        foreach (var node in nodeObjects)
        {
            unexploredNodes.Add(node.GetComponent<Node>());
        }
        foreach (Node node in unexploredNodes)
        {
            node.parentNode = null; 
        }


        Node nodeToExplore = startNode;
        unexploredNodes.Remove(startNode);  
        exploredNodes.Add(startNode);
        bool foundANode = false;

        while (!exploredNodes.Contains(endNode))
        {
            foreach (Node neighbourNode in nodeToExplore.neighbourNodes)
            {
                if (unexploredNodes.Contains(neighbourNode))
                {
                    neighbourNode.parentNode = nodeToExplore;  //set parent to be previous
                    unexploredNodes.Remove(neighbourNode);     //node has been explored
                    exploredNodes.Add(neighbourNode);
                    foundANode = true;
                }
            }
            if (!foundANode)
            {
                exploredNodes.Remove(nodeToExplore);
            }

            if (exploredNodes.Count == 0)
            {
                
                return new List<Node> { startNode };
            }

            nodeToExplore = exploredNodes[exploredNodes.Count - 1];
            foundANode = false;       
        }

        //foreach (var node in exploredNodes)
        //{
        //    Debug.Log("node: "+node);
        //    Debug.Log("parent: "+node.parentNode);
        //}
        

        List<Node> finalList = new List<Node>();
        Node nodeToInspect = endNode;
        
        foreach (var node in allNodes) //colour changin (not neccessary)
        {
            node.spriteRenderer.color = node.originalColor;
        }
        
        while (nodeToInspect.parentNode != null)
        {
            finalList.Insert(0, nodeToInspect);
            nodeToInspect.spriteRenderer.color = nodeToInspect.pathColour;
            nodeToInspect = nodeToInspect.parentNode;
        }
        //finalList.Insert(0, startNode);
        startNode.spriteRenderer.color = Color.green;   //changes colour of first node


        return finalList;
    }
}