using rccg.frontend;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace mg.pummelz
{

    public class MGPummelz : MGMinigame, MGAIConfigurable, MGLevelConfigurable
    {
        const int gameSize = 8;
        const int playerCount = 2;

        public RectTransform board;

        public MGPumChainToolTip toolTip;
        public MGPumEndTurnButton endTurnButton;
        public MGTextField textField;

        internal Dictionary<int, MGPumUnitRender> unitrenders;

        internal MGPumFieldRender[,] fieldrenders;


        internal MGPumPlayerController[] playerControllers;
        internal MGPumGameController gameController;
        internal MGPumGameConfig config;

        public MGPumInputManager inputManager;
        public MGPumStateManager stateManager;

        // Use this for initialization
        void Start()
        {
            unitrenders = new Dictionary<int, MGPumUnitRender>();
            initFields();
            reset();
        }

        private void initFields()
        {
            UnityEngine.Object fieldPrefab = GUIResourceLoader.getResourceLoaderInstance().loadMinigamePrefab("pummelz/MGPumField");

            fieldrenders = new MGPumFieldRender[gameSize, gameSize];

            for (int x = 0; x < gameSize; x++)
            {
                for (int y = 0; y < gameSize; y++)
                {
                    fieldrenders[x, y] = (Instantiate(fieldPrefab, board) as GameObject).GetComponent<MGPumFieldRender>();
                    fieldrenders[x, y].name = String.Format("Field {0}/{1}", x, y);
                    fieldrenders[x, y].transform.localPosition = gameToBoardPosition(new Vector2Int(x, y));
                }
            }
        }

        public override void reset()
        {
            stateManager.init();


            foreach (MGPumUnitRender unitR in unitrenders.Values)
            {
                if (unitR != null)
                {
                    Destroy(unitR.gameObject);
                }
            }
            unitrenders.Clear();
            foreach (MGPumFieldRender fieldR in fieldrenders)
            {
                fieldR.unitRender = null;
            }
            startGame();
        }

        public override void startGame()
        {
            if (playerControllerTypes == null)
            {
                playerControllerTypes = new string[playerCount];
                playerControllerTypes[0] = MGPumGUIPlayerController.type;
                playerControllerTypes[1] = MGPumSimpleAIPlayerController.type;
            }
            playerControllers = new MGPumPlayerController[playerCount];
            playerControllers[0] = getNewPlayerController(playerControllerTypes[0], 0);
            playerControllers[1] = getNewPlayerController(playerControllerTypes[1], 1);


            MGPumGameConfig config = new MGPumGameConfig(new MGPumPlayerConfig(), new MGPumPlayerConfig());

            config.encounter = MGPumCampaign.getInstance().getEncounter(level);

            bool hasHumanPlayer = false;
            foreach (string type in playerControllerTypes)
            {
                if (MGPumGUIPlayerController.type.Equals(type))
                {
                    hasHumanPlayer = true;
                }
            }
            if (!hasHumanPlayer)
            {
                ((MGPumAIPlayerController)playerControllers[0]).setReportingGUIController(this);
            }

            gameController = new MGPumGameController(playerControllers[0], playerControllers[1], config);

            gameController.startGame();

        }

        public void promptHumanCommand(int playerID)
        {
            inputManager.playingPlayerID = playerID;
        }

        public void visualizeEvents(List<MGPumGameEvent> eventsThatHappend, int playerID)
        {
            stateManager.enqueueEvents(eventsThatHappend);
        }

        public void setPlayerState(MGPumGameState initialState)
        {
            //we might need a separate state becxause we're only listening to an AI
            MGPumGameState copiedstate = initialState.deepCopy();
            foreach (MGPumField field in copiedstate.fields)
            {
                getFieldRender(field).init(this, field);
            }

            foreach (MGPumUnit unit in copiedstate.getAllUnitsInZone(MGPumZoneType.Battlegrounds))
            {
                createUnitRender(unit);
            }
            stateManager.setInitialState(copiedstate);
            applyTerrain(copiedstate.gameConfig.encounter.terrain);
        }

        public void applyTerrain(MGPumEncounter.Terrain terrain)
        {
            Sprite[] tiles = GUIResourceLoader.getResourceLoaderInstance().loadMinigameSprites("pummelz/tiles");
            System.Random rng = new System.Random();
            if (terrain == MGPumEncounter.Terrain.Dirt)
            {
                foreach (MGPumFieldRender fr in fieldrenders)
                {
                    fr.tile.sprite = tiles[0];
                }
            }
            if (terrain == MGPumEncounter.Terrain.Desert)
            {
                int[] tileset = new int[] {1, 8, 8, 8};
                foreach (MGPumFieldRender fr in fieldrenders)
                {

                    fr.tile.sprite = tiles[tileset[rng.Next(tileset.Length)]];
                }
            }
            if (terrain == MGPumEncounter.Terrain.Grass)
            {
                int[] tileset = new int[] { 2, 6, 6, 7, 7, 7, 7 };
                foreach (MGPumFieldRender fr in fieldrenders)
                {

                    fr.tile.sprite = tiles[tileset[rng.Next(tileset.Length)]];
                }
            }
            if (terrain == MGPumEncounter.Terrain.Flowers)
            {
                int[] tileset = new int[] { 2, 2, 2, 2, 6, 6, 6 };
                foreach (MGPumFieldRender fr in fieldrenders)
                {

                    fr.tile.sprite = tiles[tileset[rng.Next(tileset.Length)]];
                }
            }
            if (terrain == MGPumEncounter.Terrain.Ice)
            {
                int[] tileset = new int[] { 10, 10, 11 };
                foreach (MGPumFieldRender fr in fieldrenders)
                {

                    fr.tile.sprite = tiles[tileset[rng.Next(tileset.Length)]];
                }
            }
            if (terrain == MGPumEncounter.Terrain.Marble)
            {
                int[] tileset = new int[] { 9, 9, 3 };
                foreach (MGPumFieldRender fr in fieldrenders)
                {

                    fr.tile.sprite = tiles[tileset[rng.Next(tileset.Length)]];
                }
            }
            if (terrain == MGPumEncounter.Terrain.Obsidian)
            {
                int[] tileset = new int[] { 4, 4, 4 };
                foreach (MGPumFieldRender fr in fieldrenders)
                {

                    fr.tile.sprite = tiles[tileset[rng.Next(tileset.Length)]];
                }
            }
            if (terrain == MGPumEncounter.Terrain.Chess)
            {
                foreach(MGPumFieldRender fr in fieldrenders)
                {
                    fr.tile.sprite = tiles[3 + ((fr.x + fr.y) % 2)];
                }
            }
        }

        internal MGPumUnitRender createUnitRender(MGPumUnit unit)
        {
            return createUnitRender(unit, unit.field);
        }

        internal MGPumUnitRender createUnitRender(MGPumUnit unit, MGPumField field)
        {
            MGPumFieldRender fieldR = getFieldRender(field);

            UnityEngine.Object piecePrefab = GUIResourceLoader.getResourceLoaderInstance().loadMinigamePrefab("pummelz/MGPumUnit");
            MGPumUnitRender unitR = (Instantiate(piecePrefab, fieldR.transform.position, fieldR.transform.rotation, fieldR.transform) as GameObject).GetComponent<MGPumUnitRender>();
            //Debug.LogError(unitR);
            //Debug.LogError(unit.coords);
            //unitR.transform.localPosition = gameToBoardPosition(unit.coords);
            unitR.init(this, unit, fieldR);
            fieldR.unitRender = unitR;
            unitrenders.Add(unitR.unit.id, unitR);
            return unitR;
        }

        internal void fieldClicked(MGPumFieldRender fieldR)
        {
            //acceptor.fieldClicked(fieldR);
        }

        internal void pieceClicked(MGPumUnitRender pieceR)
        {
            //acceptor.pieceClicked(pieceR);
        }

        public void Update()
        {
            if (gameController != null)
            {
                gameController.checkForAsynchronousCommand();

            }
        }

        public MGPumFieldRender getFieldRender(MGPumField field)
        {
            return fieldrenders[field.x, field.y];
        }

        public MGPumFieldRender getFieldRender(Vector2Int coords)
        {
            return fieldrenders[coords.x, coords.y];
        }

        public MGPumUnitRender getUnitRender(Vector2Int coords)
        {
            return fieldrenders[coords.x, coords.y].unitRender;
        }

        public MGPumUnitRender getUnitRender(MGPumUnit unit)
        {
            return unitrenders[unit.id];
        }

        public MGPumUnitRender getUnitRender(int id)
        {
            return unitrenders[id];
        }

        public Vector3 gameToBoardPosition(Vector2Int gameCoords)
        {
            return new Vector3(8, 8 + 16 * (gameSize - 1), 0) + new Vector3(16 * gameCoords.x, -16 * gameCoords.y, 0);
        }

        //NOTE: add your AIController to these two methods to make it selectable

        private string[] playerControllerTypes;
        private string level = MGPumCampaign.TEST;

        public List<string> getAITypes()
        {
            List<string> types = new List<string>();
            types.Add(MGPumGUIPlayerController.type);
            types.Add(MGPumSkipTurnAIPlayerController.type);
            types.Add(MGPumSimpleAIPlayerController.type);
            types.Add(MGPumNobleSweetPaprikaAIPlayerController.type);

            

            return types;
        }

        private MGPumPlayerController getNewPlayerController(string type, int playerID)
        {
            if (MGPumGUIPlayerController.type.Equals(type))
            {
                //determine if playerController should be mainplayer
                Boolean multipleHumans = true;
                foreach (string ot in playerControllerTypes)
                {
                    if (!MGPumGUIPlayerController.type.Equals(ot))
                    {
                        multipleHumans = false;
                    }
                }

                
                MGPumGUIPlayerController pc = new MGPumGUIPlayerController(this, playerID);
                if (multipleHumans && playerID == 1)
                {
                    pc.nonReporting();
                }
                    

                return pc;
                
            }
            else if (MGPumSkipTurnAIPlayerController.type.Equals(type))
            {
                return new MGPumSkipTurnAIPlayerController(playerID, true);
            }
            else if (MGPumSimpleAIPlayerController.type.Equals(type))
            {
                return new MGPumSimpleAIPlayerController(playerID, true);
            }
            else if (MGPumNobleSweetPaprikaAIPlayerController.type.Equals(type))
            {
                return new MGPumNobleSweetPaprikaAIPlayerController(playerID);
            }
            else
            {
                return new MGPumSkipTurnAIPlayerController(playerID, true);
            }
        }

        public void setAIType(int playerID, string aiType)
        {
            if (playerControllerTypes == null)
            {
                playerControllerTypes = new string[playerCount];
            }
            playerControllerTypes[playerID] = aiType;
        }

        public List<string> getLevels()
        {
            List<string> levels = new List<string>();
            foreach(MGPumEncounter encounter in MGPumCampaign.getInstance().encounters)
            {
                levels.Add(encounter.id);
            }
            return levels;
        }

        public void setLevel(string level)
        {
            this.level = level;
        }

        public static string encodeSymbols(string text)
        {
            return text.Replace("{", "<sprite name=\"").Replace("}", "\">").Replace("\\n", "\n");
        }

        internal static string getColorForPlayer(int playerID)
        {
            if (playerID == 0)
            {
                return "Blue";
            }
            else
            {
                return "Red";
            }
        }

     
    }
}

