using UnityEngine;

namespace Gameplay {

    public class VehicleDestroyer {
        private const string CAR_LAYER_NAME = "Destroyable";
        private const float EXPLOSION_POWER = 300;
        private const float EXPLOSION_RADIUS = 1.5f;
        private const float OVERLAP_RADIUS = 0.7f;
        private Car _car;

        public void SetCar(Car car) {
            _car = car;
        }

        public void DestroyFormPoint(Vector3 hitPoint) {
            _car.Crash();
            DestroyMainBody(hitPoint);
            DestroySecondaryBodies(hitPoint);
        }

        private static void DestroySecondaryBodies(Vector3 hitPoint) {
            LayerMask mask = LayerMask.GetMask(CAR_LAYER_NAME);
            Collider[] colliders = new Collider[15];
            var size = Physics.OverlapSphereNonAlloc(hitPoint, OVERLAP_RADIUS, colliders, mask);

            for (var i = 0; i < size; i++) {
                var rigidbody = colliders[i].gameObject.AddComponent<Rigidbody>();
                rigidbody.AddExplosionForce(EXPLOSION_POWER, hitPoint, EXPLOSION_RADIUS, 1.0F);
            }
        }

        private void DestroyMainBody(Vector3 hitPoint) {
            _car.body.mass = 1;
            _car.body.AddExplosionForce(EXPLOSION_POWER, hitPoint, EXPLOSION_RADIUS, 1.0F);
        }
    }
}