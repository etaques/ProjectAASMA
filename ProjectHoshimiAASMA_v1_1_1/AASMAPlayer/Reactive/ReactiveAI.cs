using System;
using System.Collections.Generic;
using System.Text;
using AASMAHoshimi;
using PH.Common;
using System.Drawing;

namespace AASMAHoshimi.Reactive
{
    public class ReactiveAI : AASMAAI
    {
        public ReactiveAI(NanoAI nano)
		{
			this._nanoAI = nano;
		}
			
        public override void DoActions()
        {
            
            if (getAASMAFramework().visiblePierres(this._nanoAI).Count != 0)
            {
                List<Point> enemies = getAASMAFramework().visiblePierres(this._nanoAI);
                flee(enemies);
            }
            else  if (getAASMAFramework().protectorsAlive() < 5)
            {
                this._nanoAI.Build(typeof(ReactiveProtector),"P"+this._protectorNumber++);
            }
            else if (getAASMAFramework().containersAlive() < 5)
            {
                this._nanoAI.Build(typeof(ReactiveCollector), "C" + this._containerNumber++);
            }
            else if(getAASMAFramework().explorersAlive() < 5)
            {
                this._nanoAI.Build(typeof(ReactiveExplorer),"E"+this._explorerNumber++);
            }
            else if(!getAASMAFramework().overHoshimiPoint(this._nanoAI) || getAASMAFramework().overNeedle(this._nanoAI)) 
            {
                List<System.Drawing.Point> visibleHoshimiesList = getAASMAFramework().visibleHoshimies(this._nanoAI);
				if (visibleHoshimiesList.Count != 0) {
                    System.Drawing.Point nearestHoshimi = Utils.getNearestPoint(this._nanoAI.Location, visibleHoshimiesList);
                    if (!getAASMAFramework().visibleEmptyNeedles(this._nanoAI).Contains(nearestHoshimi) &&
                        !getAASMAFramework().visibleFullNeedles(this._nanoAI).Contains(nearestHoshimi))
                    {
                        this._nanoAI.MoveTo(nearestHoshimi);
                        return;
                    }
                }
				MoveToClearPosition(90);
            }
            else if (getAASMAFramework().overHoshimiPoint(this._nanoAI) && (!getAASMAFramework().overNeedle(this._nanoAI)))
            {
                this._nanoAI.Build(typeof(ReactiveNeedle), "N" + this._needleNumber++);
            }
        }

        public override void receiveMessage(AASMAMessage msg)
        {
        }

        private void flee(List<Point> enemies) {
            List<Point> possibleMoves = new List<Point>();
            foreach(Point enemy in enemies) {
                possibleMoves.Add(Utils.oppositDirection(this._nanoAI.Location, enemy, getAASMAFramework().Tissue));
            }
            this._nanoAI.MoveTo(Utils.getMiddlePoint(possibleMoves.ToArray()));
        }
    }
}
