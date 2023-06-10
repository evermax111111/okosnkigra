using UnityEngine;

public class BackgroundColorHandler : MonoBehaviour
{
	public Color colorADelhi;

	public Color colorADubai;

	public Color colorALondon;

	public Color colorAThailand;

	public Color colorASydney;

	public Color colorANewyork;

	public Color colorA;

	public Color colorB;

	private void Start()
	{
		if ((double)Camera.main.aspect < 0.5)
		{
			base.transform.localScale = new Vector3(10f, 20f, 1f);
		}
		else if ((double)Camera.main.aspect > 0.5)
		{
			if ((double)Camera.main.aspect >= 0.75)
			{
				base.transform.localScale = new Vector3(12f, 16f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(10f, 16f, 1f);
			}
		}
		else
		{
			base.transform.localScale = new Vector3(9f, 18f, 1f);
		}
		SpriteRenderer component = GetComponent<SpriteRenderer>();
		LevelManager instance = LevelManager.getInstance();
		switch (instance.city)
		{
		case LevelManager.City.DELHI:
			SetDelhiBackground(component);
			break;
		case LevelManager.City.DUBAI:
			SeDubaiBackground(component);
			break;
		case LevelManager.City.LONDON:
			SetLondonBackground(component);
			break;
		case LevelManager.City.THAILAND:
			SetThailandBackground(component);
			break;
		case LevelManager.City.SYDNEY:
			SetSydneyBackground(component);
			break;
		case LevelManager.City.NEWYORK:
			SetNewyorkBackground(component);
			break;
		default:
			component.sharedMaterial.SetColor("_ColorA", colorA);
			component.sharedMaterial.SetColor("_ColorB", colorB);
			break;
		}
	}

	private void SetDelhiBackground(SpriteRenderer renderer)
	{
		renderer.color= colorADelhi;
	}

	private void SeDubaiBackground(SpriteRenderer renderer)
	{
		renderer.color= colorADubai;
		
	}

	private void SetLondonBackground(SpriteRenderer renderer)
	{
		renderer.color=colorALondon;
	}

	private void SetThailandBackground(SpriteRenderer renderer)
	{
		renderer.color=colorAThailand;
	}

	private void SetSydneyBackground(SpriteRenderer renderer)
	{
		renderer.color=colorASydney;
	}

	private void SetNewyorkBackground(SpriteRenderer renderer)
	{
		renderer.color=colorANewyork;
	}

	private void Update()
	{
	}
}
