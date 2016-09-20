using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HAA.EditorUtils
{
	public class FindEmptyMonoBehaviourFunctions : EditorWindow
	{
		private enum State
		{
			SettingUp,
			Searching,
			DisplayingResults
		}

		const string defaultSearchFolder = "Assets/";
		private State currentState;
		private string searchDirectory;
		private Vector2 searchScroll;
		private Vector2 resultsScroll;
		private List<string> filesWithEmptyFunctions = new List<string>();
		private List<Regex> SearchParameters = new List<Regex>();

		[MenuItem("HAA/Utils/Search for empty MonoBehaviour functions")]
		static public void SearchFiles()
		{
			EditorWindow.GetWindow (typeof(FindEmptyMonoBehaviourFunctions));
		}

		void OnGUI()
		{
			EditorGUI.BeginDisabledGroup(currentState == State.Searching);
			if(currentState == State.SettingUp)
			{
				DisplaySearchFields();	
			}
			else if(currentState == State.DisplayingResults)
			{
				DisplayResults();	
			}
			EditorGUI.EndDisabledGroup();
		}

		private void DisplaySearchFields()
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Empty monobehaviour functions add unnecessary processing to your game. Use this search to hunt them down!");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(string.Format("Search Folder : {0}", searchDirectory));
			if(GUILayout.Button("Change Search Folder"))
			{
				searchDirectory = EditorUtility.OpenFolderPanel("Choose Search Directory", searchDirectory, "Search Directory");
			}
			EditorGUILayout.Space();
			DisplaySearchParameters();
			EditorGUILayout.Space();
			if(GUILayout.Button("Search"))
			{
				currentState = State.Searching;
				SearchForEmptyFunctions();
			}
			EditorGUILayout.EndVertical();
		}

		private void SearchForEmptyFunctions()
		{
			AddSearchParameters();
			filesWithEmptyFunctions.Clear();
			DirectoryInfo di = new DirectoryInfo(searchDirectory);
			FileInfo[] files = di.GetFiles("*.cs", SearchOption.AllDirectories);
			foreach (FileInfo file in files)
			{
				using (StreamReader sr = new StreamReader(file.FullName))
				{
					string content = sr.ReadToEnd();
					foreach(Regex searchParameter in SearchParameters)
					{
						if(searchParameter.IsMatch(content))
						{
							filesWithEmptyFunctions.Add(file.FullName);
							break;
						}
					}
				}
			}
			currentState = State.DisplayingResults;
		}

		private void DisplayResults()
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Search Results:", EditorStyles.boldLabel);
			EditorGUILayout.LabelField(string.Format("{0} Files have empty functions.", filesWithEmptyFunctions.Count));
			EditorGUILayout.Space();
			if(GUILayout.Button("Search Again"))
			{
				currentState = State.SettingUp;
			}
			EditorGUILayout.Space();
			if(filesWithEmptyFunctions.Count > 0)
			{
				resultsScroll = EditorGUILayout.BeginScrollView(resultsScroll, GUILayout.Height (500));
				foreach(string file in filesWithEmptyFunctions)
				{
					EditorGUILayout.LabelField(file);
				}
				EditorGUILayout.EndScrollView();
			}

			EditorGUILayout.EndVertical();
		}

		#region Search Display
		private void DisplaySearchParameters()
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Common Searches:", EditorStyles.boldLabel);
			ResetSelected = EditorGUILayout.Toggle("ResetSelected", ResetSelected);
			StartSelected = EditorGUILayout.Toggle("StartSelected", StartSelected);
			UpdateSelected = EditorGUILayout.Toggle("UpdateSelected", UpdateSelected);
			AwakeSelected = EditorGUILayout.Toggle("AwakeSelected", AwakeSelected);
			FixedUpdateSelected = EditorGUILayout.Toggle("FixedUpdateSelected", FixedUpdateSelected);
			LateUpdateSelected = EditorGUILayout.Toggle("LateUpdateSelected", LateUpdateSelected);
			OnDestroySelected = EditorGUILayout.Toggle("OnDestroySelected", OnDestroySelected);

			EditorGUILayout.LabelField("All Functions:", EditorStyles.boldLabel);
			searchScroll = EditorGUILayout.BeginScrollView(searchScroll, GUILayout.Height (200));
			DisplayToggleList();
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void DisplayToggleList()
		{
			OnAnimatorIKSelected = EditorGUILayout.Toggle("OnAnimatorIKSelected", OnAnimatorIKSelected);
			OnAnimatorMoveSelected = EditorGUILayout.Toggle("OnAnimatorMoveSelected", OnAnimatorMoveSelected);
			OnApplicationFocusSelected = EditorGUILayout.Toggle("OnApplicationFocusSelected", OnApplicationFocusSelected);
			OnApplicationPauseSelected = EditorGUILayout.Toggle("OnApplicationPauseSelected", OnApplicationPauseSelected);
			OnApplicationQuitSelected = EditorGUILayout.Toggle("OnApplicationQuitSelected", OnApplicationQuitSelected);
			OnAudioFilterReadSelected = EditorGUILayout.Toggle("OnAudioFilterReadSelected", OnAudioFilterReadSelected);
			OnBecameInvisibleSelected = EditorGUILayout.Toggle("OnBecameInvisibleSelected", OnBecameInvisibleSelected);
			OnBecameVisibleSelected = EditorGUILayout.Toggle("OnBecameVisibleSelected", OnBecameVisibleSelected);
			OnCollisionEnterSelected = EditorGUILayout.Toggle("OnCollisionEnterSelected", OnCollisionEnterSelected);
			OnCollisionEnter2DSelected = EditorGUILayout.Toggle("OnCollisionEnter2DSelected", OnCollisionEnter2DSelected);
			OnCollisionExitSelected = EditorGUILayout.Toggle("OnCollisionExitSelected", OnCollisionExitSelected);
			OnCollisionExit2DSelected = EditorGUILayout.Toggle("OnCollisionExit2DSelected", OnCollisionExit2DSelected);
			OnCollisionStaySelected = EditorGUILayout.Toggle("OnCollisionStaySelected", OnCollisionStaySelected);
			OnCollisionStay2DSelected = EditorGUILayout.Toggle("OnCollisionStay2DSelected", OnCollisionStay2DSelected);
			OnConnectedToServerSelected = EditorGUILayout.Toggle("OnConnectedToServerSelected", OnConnectedToServerSelected);
			OnControllerColliderHitSelected = EditorGUILayout.Toggle("OnControllerColliderHitSelected", OnControllerColliderHitSelected);
			OnDisableSelected = EditorGUILayout.Toggle("OnDisableSelected", OnDisableSelected);
			OnDisconnectedFromServerSelected = EditorGUILayout.Toggle("OnDisconnectedFromServerSelected", OnDisconnectedFromServerSelected);
			OnDrawGizmosSelected = EditorGUILayout.Toggle("OnDrawGizmosSelected", OnDrawGizmosSelected);
			OnDrawGizmosSelectedSelected = EditorGUILayout.Toggle("OnDrawGizmosSelectedSelected", OnDrawGizmosSelectedSelected);
			OnEnableSelected = EditorGUILayout.Toggle("OnEnableSelected", OnEnableSelected);
			OnFailedToConnectSelected = EditorGUILayout.Toggle("OnFailedToConnectSelected", OnFailedToConnectSelected);
			OnFailedToConnectToMasterServerSelected = EditorGUILayout.Toggle("OnFailedToConnectToMasterServerSelected", OnFailedToConnectToMasterServerSelected);
			OnGUISelected = EditorGUILayout.Toggle("OnGUISelected", OnGUISelected);
			OnJointBreakSelected = EditorGUILayout.Toggle("OnJointBreakSelected", OnJointBreakSelected);
			OnJointBreak2DSelected = EditorGUILayout.Toggle("OnJointBreak2DSelected", OnJointBreak2DSelected);
			OnMasterServerEventSelected = EditorGUILayout.Toggle("OnMasterServerEventSelected", OnMasterServerEventSelected);
			OnMouseDownSelected = EditorGUILayout.Toggle("OnMouseDownSelected", OnMouseDownSelected);
			OnMouseDragSelected = EditorGUILayout.Toggle("OnMouseDragSelected", OnMouseDragSelected);
			OnMouseEnterSelected = EditorGUILayout.Toggle("OnMouseEnterSelected", OnMouseEnterSelected);
			OnMouseExitSelected = EditorGUILayout.Toggle("OnMouseExitSelected", OnMouseExitSelected);
			OnMouseOverSelected = EditorGUILayout.Toggle("OnMouseOverSelected", OnMouseOverSelected);
			OnMouseUpSelected = EditorGUILayout.Toggle("OnMouseUpSelected", OnMouseUpSelected);
			OnMouseUpAsButtonSelected = EditorGUILayout.Toggle("OnMouseUpAsButtonSelected", OnMouseUpAsButtonSelected);
			OnNetworkInstantiateSelected = EditorGUILayout.Toggle("OnNetworkInstantiateSelected", OnNetworkInstantiateSelected);
			OnParticleCollisionSelected = EditorGUILayout.Toggle("OnParticleCollisionSelected", OnParticleCollisionSelected);
			OnParticleTriggerSelected = EditorGUILayout.Toggle("OnParticleTriggerSelected", OnParticleTriggerSelected);
			OnPlayerConnectedSelected = EditorGUILayout.Toggle("OnPlayerConnectedSelected", OnPlayerConnectedSelected);
			OnPlayerDisconnectedSelected = EditorGUILayout.Toggle("OnPlayerDisconnectedSelected", OnPlayerDisconnectedSelected);
			OnPostRenderSelected = EditorGUILayout.Toggle("OnPostRenderSelected", OnPostRenderSelected);
			OnPreCullSelected = EditorGUILayout.Toggle("OnPreCullSelected", OnPreCullSelected);
			OnPreRenderSelected = EditorGUILayout.Toggle("OnPreRenderSelected", OnPreRenderSelected);
			OnRenderImageSelected = EditorGUILayout.Toggle("OnRenderImageSelected", OnRenderImageSelected);
			OnRenderObjectSelected = EditorGUILayout.Toggle("OnRenderObjectSelected", OnRenderObjectSelected);
			OnSerializeNetworkViewSelected = EditorGUILayout.Toggle("OnSerializeNetworkViewSelected", OnSerializeNetworkViewSelected);
			OnServerInitializedSelected = EditorGUILayout.Toggle("OnServerInitializedSelected", OnServerInitializedSelected);
			OnTransformChildrenChangedSelected = EditorGUILayout.Toggle("OnTransformChildrenChangedSelected", OnTransformChildrenChangedSelected);
			OnTransformParentChangedSelected = EditorGUILayout.Toggle("OnTransformParentChangedSelected", OnTransformParentChangedSelected);
			OnTriggerEnterSelected = EditorGUILayout.Toggle("OnTriggerEnterSelected", OnTriggerEnterSelected);
			OnTriggerEnter2DSelected = EditorGUILayout.Toggle("OnTriggerEnter2DSelected", OnTriggerEnter2DSelected);
			OnTriggerExitSelected = EditorGUILayout.Toggle("OnTriggerExitSelected", OnTriggerExitSelected);
			OnTriggerExit2DSelected = EditorGUILayout.Toggle("OnTriggerExit2DSelected", OnTriggerExit2DSelected);
			OnTriggerStaySelected = EditorGUILayout.Toggle("OnTriggerStaySelected", OnTriggerStaySelected);
			OnTriggerStay2DSelected = EditorGUILayout.Toggle("OnTriggerStay2DSelected", OnTriggerStay2DSelected);
			OnValidateSelected = EditorGUILayout.Toggle("OnValidateSelected", OnValidateSelected);
			OnWillRenderObjectSelected = EditorGUILayout.Toggle("OnWillRenderObjectSelected", OnWillRenderObjectSelected);
		}

		private void AddSearchParameters()
		{
			SearchParameters.Clear();
			if(AwakeSelected) SearchParameters.Add(SearchForAwake);
			if(FixedUpdateSelected) SearchParameters.Add(SearchForFixedUpdate);
			if(LateUpdateSelected) SearchParameters.Add(SearchForLateUpdate);
			if(OnAnimatorIKSelected) SearchParameters.Add(SearchForOnAnimatorIK);
			if(OnAnimatorMoveSelected) SearchParameters.Add(SearchForOnAnimatorMove);
			if(OnApplicationFocusSelected) SearchParameters.Add(SearchForOnApplicationFocus);
			if(OnApplicationPauseSelected) SearchParameters.Add(SearchForOnApplicationPause);
			if(OnApplicationQuitSelected) SearchParameters.Add(SearchForOnApplicationQuit);
			if(OnAudioFilterReadSelected) SearchParameters.Add(SearchForOnAudioFilterRead);
			if(OnBecameInvisibleSelected) SearchParameters.Add(SearchForOnBecameInvisible);
			if(OnBecameVisibleSelected) SearchParameters.Add(SearchForOnBecameVisible);
			if(OnCollisionEnterSelected) SearchParameters.Add(SearchForOnCollisionEnter);
			if(OnCollisionEnter2DSelected) SearchParameters.Add(SearchForOnCollisionEnter2D);
			if(OnCollisionExitSelected) SearchParameters.Add(SearchForOnCollisionExit);
			if(OnCollisionExit2DSelected) SearchParameters.Add(SearchForOnCollisionExit2D);
			if(OnCollisionStaySelected) SearchParameters.Add(SearchForOnCollisionStay);
			if(OnCollisionStay2DSelected) SearchParameters.Add(SearchForOnCollisionStay2D);
			if(OnConnectedToServerSelected) SearchParameters.Add(SearchForOnConnectedToServer);
			if(OnControllerColliderHitSelected) SearchParameters.Add(SearchForOnControllerColliderHit);
			if(OnDestroySelected) SearchParameters.Add(SearchForOnDestroy);
			if(OnDisableSelected) SearchParameters.Add(SearchForOnDisable);
			if(OnDisconnectedFromServerSelected) SearchParameters.Add(SearchForOnDisconnectedFromServer);
			if(OnDrawGizmosSelected) SearchParameters.Add(SearchForOnDrawGizmos);
			if(OnDrawGizmosSelectedSelected) SearchParameters.Add(SearchForOnDrawGizmosSelected);
			if(OnEnableSelected) SearchParameters.Add(SearchForOnEnable);
			if(OnFailedToConnectSelected) SearchParameters.Add(SearchForOnFailedToConnect);
			if(OnFailedToConnectToMasterServerSelected) SearchParameters.Add(SearchForOnFailedToConnectToMasterServer);
			if(OnGUISelected) SearchParameters.Add(SearchForOnGUI);
			if(OnJointBreakSelected) SearchParameters.Add(SearchForOnJointBreak);
			if(OnJointBreak2DSelected) SearchParameters.Add(SearchForOnJointBreak2D);
			if(OnMasterServerEventSelected) SearchParameters.Add(SearchForOnMasterServerEvent);
			if(OnMouseDownSelected) SearchParameters.Add(SearchForOnMouseDown);
			if(OnMouseDragSelected) SearchParameters.Add(SearchForOnMouseDrag);
			if(OnMouseEnterSelected) SearchParameters.Add(SearchForOnMouseEnter);
			if(OnMouseExitSelected) SearchParameters.Add(SearchForOnMouseExit);
			if(OnMouseOverSelected) SearchParameters.Add(SearchForOnMouseOver);
			if(OnMouseUpSelected) SearchParameters.Add(SearchForOnMouseUp);
			if(OnMouseUpAsButtonSelected) SearchParameters.Add(SearchForOnMouseUpAsButton);
			if(OnNetworkInstantiateSelected) SearchParameters.Add(SearchForOnNetworkInstantiate);
			if(OnParticleCollisionSelected) SearchParameters.Add(SearchForOnParticleCollision);
			if(OnParticleTriggerSelected) SearchParameters.Add(SearchForOnParticleTrigger);
			if(OnPlayerConnectedSelected) SearchParameters.Add(SearchForOnPlayerConnected);
			if(OnPlayerDisconnectedSelected) SearchParameters.Add(SearchForOnPlayerDisconnected);
			if(OnPostRenderSelected) SearchParameters.Add(SearchForOnPostRender);
			if(OnPreCullSelected) SearchParameters.Add(SearchForOnPreCull);
			if(OnPreRenderSelected) SearchParameters.Add(SearchForOnPreRender);
			if(OnRenderImageSelected) SearchParameters.Add(SearchForOnRenderImage);
			if(OnRenderObjectSelected) SearchParameters.Add(SearchForOnRenderObject);
			if(OnSerializeNetworkViewSelected) SearchParameters.Add(SearchForOnSerializeNetworkView);
			if(OnServerInitializedSelected) SearchParameters.Add(SearchForOnServerInitialized);
			if(OnTransformChildrenChangedSelected) SearchParameters.Add(SearchForOnTransformChildrenChanged);
			if(OnTransformParentChangedSelected) SearchParameters.Add(SearchForOnTransformParentChanged);
			if(OnTriggerEnterSelected) SearchParameters.Add(SearchForOnTriggerEnter);
			if(OnTriggerEnter2DSelected) SearchParameters.Add(SearchForOnTriggerEnter2D);
			if(OnTriggerExitSelected) SearchParameters.Add(SearchForOnTriggerExit);
			if(OnTriggerExit2DSelected) SearchParameters.Add(SearchForOnTriggerExit2D);
			if(OnTriggerStaySelected) SearchParameters.Add(SearchForOnTriggerStay);
			if(OnTriggerStay2DSelected) SearchParameters.Add(SearchForOnTriggerStay2D);
			if(OnValidateSelected) SearchParameters.Add(SearchForOnValidate);
			if(OnWillRenderObjectSelected) SearchParameters.Add(SearchForOnWillRenderObject);
			if(ResetSelected) SearchParameters.Add(SearchForReset);
			if(StartSelected) SearchParameters.Add(SearchForStart);
			if(UpdateSelected) SearchParameters.Add(SearchForUpdate);

		}
		#endregion

		#region Search Selection
		private bool AwakeSelected;
		private bool FixedUpdateSelected;
		private bool LateUpdateSelected;
		private bool OnAnimatorIKSelected;
		private bool OnAnimatorMoveSelected;
		private bool OnApplicationFocusSelected;
		private bool OnApplicationPauseSelected;
		private bool OnApplicationQuitSelected;
		private bool OnAudioFilterReadSelected;
		private bool OnBecameInvisibleSelected;
		private bool OnBecameVisibleSelected;
		private bool OnCollisionEnterSelected;
		private bool OnCollisionEnter2DSelected;
		private bool OnCollisionExitSelected;
		private bool OnCollisionExit2DSelected;
		private bool OnCollisionStaySelected;
		private bool OnCollisionStay2DSelected;
		private bool OnConnectedToServerSelected;
		private bool OnControllerColliderHitSelected;
		private bool OnDestroySelected;
		private bool OnDisableSelected;
		private bool OnDisconnectedFromServerSelected;
		private bool OnDrawGizmosSelected;
		private bool OnDrawGizmosSelectedSelected;
		private bool OnEnableSelected;
		private bool OnFailedToConnectSelected;
		private bool OnFailedToConnectToMasterServerSelected;
		private bool OnGUISelected;
		private bool OnJointBreakSelected;
		private bool OnJointBreak2DSelected;
		private bool OnMasterServerEventSelected;
		private bool OnMouseDownSelected;
		private bool OnMouseDragSelected;
		private bool OnMouseEnterSelected;
		private bool OnMouseExitSelected;
		private bool OnMouseOverSelected;
		private bool OnMouseUpSelected;
		private bool OnMouseUpAsButtonSelected;
		private bool OnNetworkInstantiateSelected;
		private bool OnParticleCollisionSelected;
		private bool OnParticleTriggerSelected;
		private bool OnPlayerConnectedSelected;
		private bool OnPlayerDisconnectedSelected;
		private bool OnPostRenderSelected;
		private bool OnPreCullSelected;
		private bool OnPreRenderSelected;
		private bool OnRenderImageSelected;
		private bool OnRenderObjectSelected;
		private bool OnSerializeNetworkViewSelected;
		private bool OnServerInitializedSelected;
		private bool OnTransformChildrenChangedSelected;
		private bool OnTransformParentChangedSelected;
		private bool OnTriggerEnterSelected;
		private bool OnTriggerEnter2DSelected;
		private bool OnTriggerExitSelected;
		private bool OnTriggerExit2DSelected;
		private bool OnTriggerStaySelected;
		private bool OnTriggerStay2DSelected;
		private bool OnValidateSelected;
		private bool OnWillRenderObjectSelected;
		private bool ResetSelected;
		private bool StartSelected;
		private bool UpdateSelected;
		#endregion

		#region Regex Search
		private Regex SearchForAwake = new Regex(@"void\s*Awake\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForFixedUpdate = new Regex(@"void\s*FixedUpdate\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForLateUpdate = new Regex(@"void\s*LateUpdate\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnAnimatorIK = new Regex(@"void\s*OnAnimatorIK\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnAnimatorMove = new Regex(@"void\s*OnAnimatorMove\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnApplicationFocus = new Regex(@"void\s*OnApplicationFocus\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnApplicationPause = new Regex(@"void\s*OnApplicationPause\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnApplicationQuit = new Regex(@"void\s*OnApplicationQuit\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnAudioFilterRead = new Regex(@"void\s*OnAudioFilterRead\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnBecameInvisible = new Regex(@"void\s*OnBecameInvisible\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnBecameVisible = new Regex(@"void\s*OnBecameVisible\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnCollisionEnter = new Regex(@"void\s*OnCollisionEnter\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnCollisionEnter2D = new Regex(@"void\s*OnCollisionEnter2D\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnCollisionExit = new Regex(@"void\s*OnCollisionExit\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnCollisionExit2D = new Regex(@"void\s*OnCollisionExit2D\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnCollisionStay = new Regex(@"void\s*OnCollisionStay\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnCollisionStay2D = new Regex(@"void\s*OnCollisionStay2D\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnConnectedToServer = new Regex(@"void\s*OnConnectedToServer\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnControllerColliderHit = new Regex(@"void\s*OnControllerColliderHit\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnDestroy = new Regex(@"void\s*OnDestroy\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnDisable = new Regex(@"void\s*OnDisable\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnDisconnectedFromServer = new Regex(@"void\s*OnDisconnectedFromServer\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnDrawGizmos = new Regex(@"void\s*OnDrawGizmos\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnDrawGizmosSelected = new Regex(@"void\s*OnDrawGizmosSelected\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnEnable = new Regex(@"void\s*OnEnable\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnFailedToConnect = new Regex(@"void\s*OnFailedToConnect\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnFailedToConnectToMasterServer = new Regex(@"void\s*OnFailedToConnectToMasterServer\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnGUI = new Regex(@"void\s*OnGUI\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnJointBreak = new Regex(@"void\s*OnJointBreak\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnJointBreak2D = new Regex(@"void\s*OnJointBreak2D\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMasterServerEvent = new Regex(@"void\s*OnMasterServerEvent\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMouseDown = new Regex(@"void\s*OnMouseDown\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMouseDrag = new Regex(@"void\s*OnMouseDrag\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMouseEnter = new Regex(@"void\s*OnMouseEnter\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMouseExit = new Regex(@"void\s*OnMouseExit\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMouseOver = new Regex(@"void\s*OnMouseOver\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMouseUp = new Regex(@"void\s*OnMouseUp\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnMouseUpAsButton = new Regex(@"void\s*OnMouseUpAsButton\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnNetworkInstantiate = new Regex(@"void\s*OnNetworkInstantiate\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnParticleCollision = new Regex(@"void\s*OnParticleCollision\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnParticleTrigger = new Regex(@"void\s*OnParticleTrigger\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnPlayerConnected = new Regex(@"void\s*OnPlayerConnected\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnPlayerDisconnected = new Regex(@"void\s*OnPlayerDisconnected\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnPostRender = new Regex(@"void\s*OnPostRender\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnPreCull = new Regex(@"void\s*OnPreCull\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnPreRender = new Regex(@"void\s*OnPreRender\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnRenderImage = new Regex(@"void\s*OnRenderImage\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnRenderObject = new Regex(@"void\s*OnRenderObject\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnSerializeNetworkView = new Regex(@"void\s*OnSerializeNetworkView\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnServerInitialized = new Regex(@"void\s*OnServerInitialized\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTransformChildrenChanged = new Regex(@"void\s*OnTransformChildrenChanged\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTransformParentChanged = new Regex(@"void\s*OnTransformParentChanged\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTriggerEnter = new Regex(@"void\s*OnTriggerEnter\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTriggerEnter2D = new Regex(@"void\s*OnTriggerEnter2D\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTriggerExit = new Regex(@"void\s*OnTriggerExit\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTriggerExit2D = new Regex(@"void\s*OnTriggerExit2D\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTriggerStay = new Regex(@"void\s*OnTriggerStay\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnTriggerStay2D = new Regex(@"void\s*OnTriggerStay2D\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnValidate = new Regex(@"void\s*OnValidate\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForOnWillRenderObject = new Regex(@"void\s*OnWillRenderObject\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForReset = new Regex(@"void\s*Reset\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForStart = new Regex(@"void\s*Start\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}"); 
		private Regex SearchForUpdate = new Regex(@"void\s*Update\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}");
		#endregion
	}
}

