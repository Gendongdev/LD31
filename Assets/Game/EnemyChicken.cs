using UnityEngine;
using System.Collections;

public class EnemyChicken : Enemy {

	protected override string OnHitParticleSystemName () {
		return "ChickenFeathersParticles";
	}



}
