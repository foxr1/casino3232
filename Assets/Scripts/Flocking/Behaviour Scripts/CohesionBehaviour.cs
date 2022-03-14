using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbours, return no adjustment
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        // Add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += new Vector3(item.position.x, 0, item.position.z);
        }
        cohesionMove /= context.Count;

        // Create offset from agent position
        cohesionMove -= new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
        return cohesionMove;
    }
}
