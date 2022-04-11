using RelegatiaCCG.rccg.engine.state;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGameState
    {
        internal IDManager idm;

        public MGPumPlayer[] players { get; set; }

        public MGPumFields fields;

        public bool gameRunning { get; set; }
        public bool gameFinished { get; set; }
        public bool gameConceded { get; set; }

        public int lastChangeTurnNumber { get; set; }
        public int turnNumber { get; set; }
        public int activePlayer { get; set; }
        public int regularTurnPlayer { get; set; }
        public int[] playerTurnNumber { get; set; }

        public MGPumGameResultType result { get; set; }

        private Queue<MGPumEffectQueueItem> effectQueue;

        internal Queue<int> extraTurnQueue;

        public List<MGPumOngoingAbility> ongoingAbilities;

        public MGPumGameConfig gameConfig;

        internal MGPumGameLog log;

        private Dictionary<int, MGPumUnit> unitDictionary;

        public System.Random rng;

        private MGPumGameState()
        {
            this.rng = new System.Random();

            this.idm = new IDManager();

            this.extraTurnQueue = new Queue<int>();

        }

        internal MGPumGameState(MGPumGameConfig gc) : this()
        {
            this.gameConfig = gc;

            this.unitDictionary = new Dictionary<int, MGPumUnit>();

            this.effectQueue = new Queue<MGPumEffectQueueItem>();

            players = new MGPumPlayer[2];
            players[0] = new MGPumPlayer(0, gc.getPlayerConfig(0));
            players[1] = new MGPumPlayer(1, gc.getPlayerConfig(1));

            this.fields = new MGPumFields(idm);

            int y = 0;
            foreach(String row in gc.encounter.battlegrounds)
            {
                for(int x = 0; x < gc.columns; x++)
                {
                    char unitc = row[x];
                    
                    if (Char.IsUpper(unitc)|| Char.IsLower(unitc))
                    {
                        int owner = Char.IsUpper(unitc) ? 1 : 0;
                        String unitID = gc.encounter.unitDictionary[Char.ToLower(unitc)];
                        MGPumUnit unit = createUnit(unitID, owner);
                        unit.zone = MGPumZoneType.Battlegrounds;
                        fields.getField(x, y).unit = unit;
                    }
                }

                y++;
            }

            this.gameRunning = false;
            this.gameConceded = false;
            this.turnNumber = 0;
            this.playerTurnNumber = new int[2] { 0, 0 };
            this.result = MGPumGameResultType.NotYet;

            this.log = new MGPumGameLog();
        }

        public MGPumGameState getInitialStateForPlayer(int playerID)
        {
            MGPumGameState gameState = this.deepCopy();

            return gameState;
        }

        public MGPumGameState deepCopy()
        {
            MGPumGameState gameState = new MGPumGameState();

            gameState.gameRunning = this.gameRunning;
            gameState.gameFinished = this.gameFinished;
            gameState.gameConceded = this.gameConceded;

            gameState.turnNumber = this.turnNumber;
            gameState.activePlayer = this.activePlayer;
            gameState.regularTurnPlayer = this.regularTurnPlayer;
            gameState.playerTurnNumber = new int[2] { this.playerTurnNumber[0], this.playerTurnNumber[1] };
            gameState.result = this.result;

            gameState.gameConfig = this.gameConfig.deepCopy();

            gameState.unitDictionary = new Dictionary<int, MGPumUnit>();
            foreach(MGPumUnit c in this.unitDictionary.Values)
            {
                MGPumUnit copy = c.deepCopy();
                gameState.unitDictionary.Add(copy.id, copy);
            }

            foreach (int playerID in this.extraTurnQueue)
            {
                gameState.extraTurnQueue.Enqueue(playerID);
            }

            gameState.players = new MGPumPlayer[2];
            gameState.players[0] = this.players[0].deepCopy(gameState);
            gameState.players[1] = this.players[1].deepCopy(gameState);

            gameState.fields = this.fields.deepCopy(gameState);

            //log may access anything copied before 
            //log may also access itself over gamestate, so it needs to be assigned to new gamestate before deepcopy
            gameState.log = new MGPumGameLog();
            this.log.deepCopyToGameLog(gameState.log, gameState);

            //effect queue may access event log
            gameState.effectQueue = new Queue<MGPumEffectQueueItem>();
            Queue<MGPumEffectQueueItem> copyQueue = new Queue<MGPumEffectQueueItem>(this.effectQueue);
            while(copyQueue.Any())
            {
                MGPumEffectQueueItem i = copyQueue.Dequeue();
                MGPumEffectQueueItem copy = i.deepCopy(gameState);
                

                gameState.effectQueue.Enqueue(copy);
            }

            gameState.idm = this.idm.deepCopy();

            return gameState;
        }


        internal MGPumUnit lookupOrCreate(MGPumUnit unit)
        {
            if(unit == null)
            {
                return null;
            }

            MGPumUnit internalUnit = lookupUnit(unit.id);
           
            if (internalUnit == null)
            {
                internalUnit = unit.deepCopy();
                unitDictionary.Add(internalUnit.id, internalUnit);
            }

            return internalUnit;
        }

        internal MGPumGameEvent lookupEvent(MGPumGameEvent e)
        {
            if(e == null)
            {
                Debug.LogError("Cannot lookup null event");
            }
            return log.getEventByNumber(e.number);
        }

        internal MGPumEffect lookupEffect(MGPumEffect effect, MGPumEntity source)
        {
            if(source is MGPumAbility)
            {
                return lookupEffectAbility(effect, (MGPumAbility)source);
            }
            else if (source is MGPumUnit)
            {
                MGPumEffect result = null;
                result = lookupEffectAbility(effect, ((MGPumUnit)source).abilityPrinted);
                
                if (result == null)
                {
                    result = lookupEffectAbility(effect, ((MGPumUnit)source).abilityBase);
                }
                if (result == null)
                {
                    result = lookupEffectAbility(effect, ((MGPumUnit)source).abilityCurrent);
                }
                return result;
            }
            else
            {
                throw new NotImplementedException("Cannot lookup effect from entity:" + source);
            }
        }

        internal MGPumEffect lookupOrCreateEffect(MGPumEffect effect, MGPumGameState state, MGPumEntity source)
        {
            MGPumEffect e = lookupEffect(effect, source);

            if(e == null)
            {
                e = effect.deepCopy(state);
            }
            return e;

        }

        internal MGPumEffect lookupEffectAbility(MGPumEffect effect, MGPumAbility ability)
        {
            MGPumEffect result = null;
            if (ability is MGPumOngoingAbility)
            {
                MGPumOngoingAbility oa = (MGPumOngoingAbility)ability;
                if (oa.effect.id == effect.id)
                {
                    result = oa.effect;
                }
               

            }
            else if (ability is MGPumTriggeredAbility)
            {
                MGPumTriggeredAbility ta = (MGPumTriggeredAbility)ability;
                if (ta.effect.id == effect.id)
                {
                    result = ta.effect;
                }

            }
            else if (ability == null)
            {
            }
            else
            {
                throw new NotImplementedException("Cannot lookup effect from ability: " + ability);
            }
            return result;
        }

        internal MGPumAbility lookupAbility(MGPumAbility ability, MGPumUnit origin)
        {
            MGPumUnit p = (MGPumUnit)lookupUnit(origin.id);
            if (p.abilityPrinted.id == ability.id)
            {
                return p.abilityPrinted;
            }
            if (p.abilityBase.id == ability.id)
            {
                return p.abilityBase;
            }
            if (p.abilityCurrent.id == ability.id)
            {
                return p.abilityCurrent;
            }
            return null;
        }


        internal MGPumUnit createUnit(string unitID, int ownerID)
        {
            int id = idm.getNextID();

            MGPumUnit unit = MGPumSetHandler.getInstance().getNewUnit(id, unitID, ownerID, idm);
            unitDictionary.Add(id, unit);
            return unit;
        }

        internal MGPumUnit createUnit(int id, string unitID, int ownerID)
        {
            
            MGPumUnit unit = MGPumSetHandler.getInstance().getNewUnit(id, unitID, ownerID, idm);
            unitDictionary.Add(id, unit);
            return unit;
        }

        internal void createdUnit(MGPumUnit unit)
        {
            int id = idm.getNextID();

            if (id != unit.id)
            {
                Debug.LogError("Mismatch between id " + id + " and expected id for created unit " + unit.id + " !");
            }

            MGPumAbility a = unit.abilityPrinted;
            if(a != null)
            {
                int aid = idm.getNextID();
                if (aid != a.id)
                {
                    Debug.LogError("Mismatch between id " + aid + " and expected ability id " + a.id + " for created unit " + unit.id + " !");
                }
                //TODO: this resets effect IDS but should be the same
                a.setEffectIDs(idm);
            }
        }

        public MGPumAbility copyAbility(MGPumAbility a)
        {
            MGPumAbility copiedA = a.deepCopy();
            copiedA.id = idm.getNextID();
            copiedA.setEffectIDs(idm);
            copiedA.owner = lookupEntity(a.owner);
            return copiedA;
        }

        public MGPumEffect copyEffect(MGPumEffect e)
        {
            MGPumEffect copiedE = e.deepCopy(this);
            copiedE.id = idm.getNextID();
            copiedE.setEffectIDs(idm);
            return copiedE;
        }

        
        internal MGPumEntity lookupEntity(MGPumEntity entity)
        {
            if(entity == null)
            {
                Debug.LogWarning("Looking up entity null.");
                return null;
            }

            if(entity is MGPumUnit)
            {
                return lookupUnit(entity.id);
            }
            else if (entity is MGPumField)
            {
                return getField((MGPumField)entity);
            }
            else if (entity is MGPumPlayer)
            {
                return getPlayer(entity.id);
            }
            else
            {
                throw new NotImplementedException("No lookup for game entity:" + entity);
            }
        }

        internal MGPumUnit lookupUnit(int id)
        {
            if(unitDictionary.ContainsKey(id))
            {
                return unitDictionary[id];
            }
            else
            {
                //Debug.LogError("Looking up and not found " + id);
                return null;
            }

        }

        public void registerOngoingAbilities()
        {
            this.ongoingAbilities = new List<MGPumOngoingAbility>();
            
            foreach (MGPumUnit unit in this.getAllUnitsInZone(MGPumZoneType.Battlegrounds))
            {
                MGPumAbility a =unit.abilityCurrent;
                {
                    if (a is MGPumOngoingAbility)
                    {
                        this.ongoingAbilities.Add((MGPumOngoingAbility)a);
                    }
                }
            }
        }

        public MGPumPlayer getOpponent(int playerID)
        {
            if(playerID == MGPumGameController.PLAYER1)
            {
                return getPlayer(MGPumGameController.PLAYER0);

            }
            else
            {
                return getPlayer(MGPumGameController.PLAYER1);
            }
        }

        public void queueEffect(MGPumEffectQueueItem e)
        {
            this.effectQueue.Enqueue(e);
        }

        public MGPumEffectQueueItem getNextEffect()
        {
            return this.effectQueue.First();
        }

        public bool effectQueueEmpty()
        {
            return !effectQueue.Any();
        }

        public MGPumEffectQueueItem dequeueNextQueuedEffect()
        {
            return this.effectQueue.Dequeue();
        }

        public MGPumZone getZone(MGPumZoneType zt, int playerID)
        {
            if (zt == MGPumZoneType.Battlegrounds)
            {
                return fields;
            }
            else if (zt == MGPumZoneType.Destroyed)
            {
                return getPlayer(playerID).destroyedZone;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public MGPumPlayer getActivePlayer()
        {
            return players[activePlayer];
        }

        public MGPumPlayer getPlayer(int playerID)
        {
            if(playerID != 0 && playerID != 1)
            {
                Debug.LogError("playerID" + playerID);
            }
            return players[playerID];
        }

        public MGPumField getField(MGPumField fieldToLookup)
        {
            if(fieldToLookup == null)
            {
                return null;
            }
            return fields.getField(fieldToLookup.coords);
        }

        public MGPumField getField(Vector2Int coords)
        {
            if (!fields.inBounds(coords))
            {
                return null;
            }
            return fields.getField(coords);
        }

        public List<MGPumUnit> getAllUnitsInZone(MGPumZoneType zoneType, int playerID)
        {
            if (zoneType == MGPumZoneType.Destroyed)
            {
                List<MGPumUnit> units = new List<MGPumUnit>();

                units.AddRange(this.getPlayer(playerID).destroyedZone.units);

                return units;
            }
            else if (zoneType == MGPumZoneType.Battlegrounds)
            {
                List<MGPumUnit> units = new List<MGPumUnit>();

                    foreach (MGPumField f in fields)
                    {
                        if (f.unit != null && playerID == f.unit.ownerID)
                        {
                            units.Add(f.unit);
                        }
                    }
                return units;
            }
            else
            {
                throw new NotImplementedException("Cannot get all units from zone: " + zoneType);
            }
        }

        public IEnumerable<MGPumUnit> getAllUnitsInZone(MGPumZoneType zoneType)
        {
            if (zoneType == MGPumZoneType.Destroyed)
            {
                List<MGPumUnit> units = new List<MGPumUnit>();

                foreach (MGPumPlayer p in players)
                {
                    units.AddRange(p.destroyedZone.units);
                }

                return units;
            }
            else if (zoneType == MGPumZoneType.Battlegrounds)
            {
                List<MGPumUnit> units = new List<MGPumUnit>();

                
                foreach(MGPumField f in fields)
                {
                    if (f.unit != null)
                    {
                        units.Add(f.unit);
                    }
                }
                return units;
            }
            else
            {
                throw new NotImplementedException("Cannot get all units from zone: " + zoneType);
            }
        }

        public MGPumField getFieldForUnit(MGPumUnit unit)
        {
            MGPumField result = null;

            foreach (MGPumField f in fields)
            {
                if (f.unit == unit)
                {
                    result = f;
                    break;
                }
            }
            return result;
        }

        public MGPumUnit getUnitForField(MGPumField field)
        {
            MGPumUnit result = null;

            MGPumField fieldInState = getField(field);

            if(fieldInState != null)
            {
                result = fieldInState.unit;
            }
            return result;
        }

        public void concede(int playerID)
        {
            this.gameRunning = false;
            this.gameFinished = true;
            this.gameConceded = true;
            if(playerID == 1)
            {
                this.result = MGPumGameResultType.WinPlayer0;
            }
            else
            {
                this.result = MGPumGameResultType.WinPlayer1;
            }
            
        }

    }
}
