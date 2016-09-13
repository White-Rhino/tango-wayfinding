using System.Collections;
using UnityEngine;
using Tango;

public class AreaLearningStartup : MonoBehaviour, ITangoLifecycle
{
	private TangoApplication m_tangoApplication;

	/// <summary>
	/// The overlay image of the relocalization process.
	/// </summary>
//	public GameObject m_relocalizationOverlay;
//
//	Canvas cv = GetComponent<>(CanvasUI);
//	Rigidbody rb = GetComponent<Rigidbody>();
//
//	// Change the mass of the object's Rigidbody.
//	rb.mass = 10f;

	public void Start()
	{
		m_tangoApplication = FindObjectOfType<TangoApplication>();
		if (m_tangoApplication != null)
		{
			m_tangoApplication.Register(this);
			m_tangoApplication.RequestPermissions();
		}
	}

	public void OnTangoPermissions(bool permissionsGranted)
	{
		if (permissionsGranted)
		{
			AreaDescription[] list = AreaDescription.GetList();
			AreaDescription mostRecent = null;
			AreaDescription.Metadata mostRecentMetadata = null;
			if (list.Length > 0)
			{
				// Find and load the most recent Area Description
				mostRecent = list[0];
				mostRecentMetadata = mostRecent.GetMetadata();
				foreach (AreaDescription areaDescription in list)
				{
					AreaDescription.Metadata metadata = areaDescription.GetMetadata();
					if (metadata.m_dateTime > mostRecentMetadata.m_dateTime)
					{
						mostRecent = areaDescription;
						mostRecentMetadata = metadata;
					}
				}

				m_tangoApplication.Startup(mostRecent);
			}
			else
			{
				// No Area Descriptions available.
				Debug.Log("No area descriptions available.");
				m_tangoApplication.Startup(null);
			}
		}
	}
}