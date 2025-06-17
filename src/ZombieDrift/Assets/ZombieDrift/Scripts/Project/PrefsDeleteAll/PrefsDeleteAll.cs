using UnityEngine;

[ExecuteAlways]
public class PrefsDeleteAll : MonoBehaviour
{
	public void DeleteAllPrefs() =>
			PlayerPrefs.DeleteAll();
}
