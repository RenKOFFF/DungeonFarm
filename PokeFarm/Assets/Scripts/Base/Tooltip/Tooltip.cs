using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Tooltip : MonoBehaviour {

	public static string text;
	public static bool isUI;

	public Color BGColor = Color.white;
	public Color textColor = Color.black;
	public int fontSize = 14; // размер шрифта
	public int maxWidth = 250; // максимальная ширина Tooltip
	public int border = 10; // ширина обводки
	public RectTransform background;
	public RectTransform arrow;
	public TextMeshProUGUI boxText;
	public Camera _camera;
	public float speed = 10; // скорость плавного затухания и проявления

	private Image[] img;
	private Color BGColorFade;
	private Color textColorFade;
	
	[SerializeField] private GameObject _commandStationUI;

	void Awake()
	{
		img = new Image[2];
		img[0] = background.GetComponent<Image>();
		img[1] = arrow.GetComponent<Image>();
		background.sizeDelta = new Vector2(maxWidth, background.sizeDelta.y);
		BGColorFade = BGColor;
		BGColorFade.a = 0;
		textColorFade = textColor;
		textColorFade.a = 0;
		isUI = false;
		foreach(Image bg in img)
		{
			bg.color = BGColorFade;
		}
		boxText.color = textColorFade;
		boxText.alignment = TextAlignmentOptions.Midline;
	}

	void FixedUpdate()
	{
		bool show = false;
		boxText.fontSize = fontSize;
		
		RaycastHit2D hit = Physics2D.Raycast(
			_camera.ScreenToWorldPoint(Input.mousePosition),
			Vector2.zero);
		if(hit.transform != null)
		{
			if(hit.transform.GetComponent<TooltipTextUI>() || hit.transform.GetComponentInParent<TooltipTextUI>())
			{
				text = hit.transform.GetComponent<TooltipTextUI>().Text;
				show = true;
			}
		}
		
		boxText.text = text;
		float width = maxWidth;
		if(boxText.preferredWidth <= maxWidth - border) width = boxText.preferredWidth + border;
		background.sizeDelta = new Vector2(width, boxText.preferredHeight + border);

		float arrowShift = width / 4; // сдвиг позиции стрелки по Х

		if(_commandStationUI && _commandStationUI.activeInHierarchy && (show || isUI))
		{
			float arrowPositionY = -(arrow.sizeDelta.y / 2 - 1); // позиция стрелки по умолчанию (внизу)
			float arrowPositionX = arrowShift;
			
			float curY = Input.mousePosition.y + background.sizeDelta.y / 2 + arrow.sizeDelta.y;
			Vector3 arrowScale = new Vector3(1, 1, 1);
			if(curY + background.sizeDelta.y / 2 > Screen.height) // если Tooltip выходит за рамки экрана, в данном случаи по высоте
			{
				curY = Input.mousePosition.y - background.sizeDelta.y / 2 - arrow.sizeDelta.y;
				arrowPositionY = background.sizeDelta.y + (arrow.sizeDelta.y / 2 - 1);
				arrowScale = new Vector3(1, -1, 1); // отражение по вертикале
			}
			
			float curX = Input.mousePosition.x + arrowShift;
			if(curX + background.sizeDelta.x / 2 > Screen.width)
			{
				curX = Input.mousePosition.x - arrowShift;
				arrowPositionX = width - arrowShift;
			}
			
			background.anchoredPosition = new Vector2(curX, curY);
			
			arrow.anchoredPosition = new Vector2(arrowPositionX, arrowPositionY);
			arrow.localScale = arrowScale;

			foreach(Image bg in img)
			{
				bg.color = Color.Lerp(bg.color, BGColor, speed * Time.deltaTime);
			}
			boxText.color = Color.Lerp(boxText.color, textColor, speed * Time.deltaTime);
		}
		else
		{
			foreach(Image bg in img)
			{
				bg.color = Color.Lerp(bg.color, BGColorFade, speed * Time.deltaTime);
			}
			boxText.color = Color.Lerp(boxText.color, textColorFade, speed * Time.deltaTime);
		}
	}
}