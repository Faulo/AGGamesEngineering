using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace AGGE.LINQ {
    public class ThingSpawner : MonoBehaviour {
        enum SelectionMode {
            Nothing,
            AllCubes,
            SecondFiveSpheres,
            FirstElement,
            RandomElement,
            FirstOfEachForm,
            FirstOfEachForm2,
        }
        [SerializeField]
        Thing prefab = default;
        [SerializeField, Range(0, 1000)]
        int count = 100;
        [SerializeField]
        SelectionMode selection = SelectionMode.Nothing;
        [SerializeField]
        List<Thing> instances = new List<Thing>();

        int randomPosition => Random.Range(0, 10);

        protected void Start() {
            for (int i = 0; i < count; i++) {
                var instance = CreateInstance();
                instances.Add(instance);
            }
        }

        Thing CreateInstance() {
            var instance = Instantiate(prefab);

            instance.color = new Color(Random.value, Random.value, Random.value);
            instance.position = new Vector3Int(randomPosition, randomPosition, randomPosition);
            instance.form = System.Enum.GetValues(typeof(PrimitiveType))
                .OfType<PrimitiveType>()
                .RandomElement();

            return instance;
        }

        void Update() {
            foreach (var instance in instances) {
                instance.isSelected = false;
            }
            foreach (var instance in GetSelectedInstaces()) {
                instance.isSelected = true;
            }
        }
        IEnumerable<Thing> GetSelectedInstaces() {
            return selection switch {
                SelectionMode.Nothing => Enumerable.Empty<Thing>(),
                SelectionMode.AllCubes => instances
                    .Where(IsCube),
                SelectionMode.SecondFiveSpheres => instances
                    .Where(thing => thing.form == PrimitiveType.Sphere)
                    .Skip(5)
                    .Take(5),
                SelectionMode.FirstElement => new[] { instances.First() },
                SelectionMode.RandomElement => new[] { instances.RandomElement() },
                SelectionMode.FirstOfEachForm => instances
                    .GroupBy(thing => thing.form)
                    .Select(group => group.First()),
                SelectionMode.FirstOfEachForm2 => Enumerable.Range(0, 6)
                    .Cast<PrimitiveType>()
                    .Select(type => instances.First(thing => thing.form == type)),
                _ => throw new System.NotImplementedException(),
            };
        }

        PrimitiveType GroupByForm(Thing thing) {
            return thing.form;
        }
        IEnumerable<Thing> SelectAll(IGrouping<PrimitiveType, Thing> group, int index) {
            return group;
        }

        Dictionary<PrimitiveType, List<Thing>> GroupBy(IEnumerable<Thing> things, PrimitiveType type) {
            var dictionary = new Dictionary<PrimitiveType, List<Thing>>();
            foreach (var thing in things) {
                if (!dictionary.ContainsKey(thing.form)) {
                    dictionary[thing.form] = new List<Thing>();
                }
                dictionary[thing.form].Add(thing);
            }
            return dictionary;
        }

        bool IsCube(Thing thing) {
            return thing.form == PrimitiveType.Cube;
        }
    }
}
