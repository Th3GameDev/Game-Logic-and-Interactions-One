using UnityEngine;
using System.Collections;
using UnityEngine.AI;

// Enumeration to define different movement methods for OffMeshLink traversal.
public enum OffMeshLinkMoveMethod
{
    Teleport,    // Instantly teleport to the destination.
    NormalSpeed, // Move at normal speed to the destination.
    Parabola,    // Move using a parabolic arc to the destination.
    Curve        // Move using a custom curve to the destination.
}

// This script handles the movement of the agent when traversing OffMeshLinks.
[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola; // The selected movement method.
    public AnimationCurve curve = new AnimationCurve(); // Custom curve for the Curve movement method.

    // Coroutine that starts when the script is initialized.
    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false; // Disable auto-traversal of OffMeshLinks.

        // Continuously check if the agent is on an OffMeshLink and handle the movement accordingly.
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                // Based on the selected movement method, call the corresponding coroutine.
                if (method == OffMeshLinkMoveMethod.NormalSpeed)
                {
                    yield return StartCoroutine(NormalSpeed(agent));
                }
                else if (method == OffMeshLinkMoveMethod.Parabola)
                {
                    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                }
                else if (method == OffMeshLinkMoveMethod.Curve)
                {
                    yield return StartCoroutine(Curve(agent, 0.5f));
                }

                agent.CompleteOffMeshLink(); // Signal that the OffMeshLink traversal is complete.
            }
            yield return null;
        }
    }

    // Coroutine for moving the agent at normal speed to the destination OffMeshLink point.
    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        // Get the data for the current OffMeshLink the agent is on.
        OffMeshLinkData data = agent.currentOffMeshLinkData;

        // Calculate the destination position with an offset for height to avoid clipping with the ground.
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        // Continue the loop until the agent reaches the destination position.
        while (agent.transform.position != endPos)
        {
            // Move the agent towards the destination position at its normal speed.
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);

            // Yield null to allow other coroutines to run, and wait for the next frame.
            yield return null;
        }
    }

    // Coroutine for moving the agent in a parabolic arc to the destination OffMeshLink point.
    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        // Get the data for the current OffMeshLink the agent is on.
        OffMeshLinkData data = agent.currentOffMeshLinkData;

        // Define the starting position of the agent and the destination position with an offset for height.
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float normalizedTime = 0.0f; // Normalized time for the parabolic movement.

        // Continue the loop until the normalized time reaches 1.0f (end of the parabola).
        while (normalizedTime < 1.0f)
        {
            // Calculate the height offset for the parabolic arc based on the normalized time.
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);

            // Move the agent along the parabolic arc using Lerp.
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;

            // Update the normalized time based on the elapsed time and the specified duration.
            normalizedTime += Time.deltaTime / duration;

            // Yield null to allow other coroutines to run, and wait for the next frame.
            yield return null;
        }
    }

    // Coroutine for moving the agent using a custom curve to the destination OffMeshLink point.
    IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        // Get the data for the current OffMeshLink the agent is on.
        OffMeshLinkData data = agent.currentOffMeshLinkData;

        // Define the starting position of the agent and the destination position with an offset for height.
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float normalizedTime = 0.0f; // Normalized time for the curve movement.

        // Continue the loop until the normalized time reaches 1.0f (end of the curve).
        while (normalizedTime < 1.0f)
        {
            // Evaluate the custom curve at the normalized time to get the height offset.
            float yOffset = curve.Evaluate(normalizedTime);

            // Move the agent along the curve using Lerp.
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;

            // Update the normalized time based on the elapsed time and the specified duration.
            normalizedTime += Time.deltaTime / duration;

            // Yield null to allow other coroutines to run, and wait for the next frame.
            yield return null;
        }
    }
}