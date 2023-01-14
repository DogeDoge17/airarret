using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT.Collision
{
    public class Collider
    {
        public Rectangle bounds = new Rectangle(0, 0, 16, 16);

        public delegate void OnCollide();

        public HitInfo hits = new HitInfo();

        public string id = "";


        public Collider()
        {
            CollisionManagement.colliders.Add(this);
        }

        public Collider(Rectangle rect)
        {

            bounds = rect;
            CollisionManagement.colliders.Add(this);
        }

        public Collider(int x, int y, int width, int height)
        {

            bounds = new Rectangle(x, y, width, height);
            CollisionManagement.colliders.Add(this);
        }

    }

    public class DynamicCollider
    {
        public DynamicRectangle dynamicRectangle = new DynamicRectangle(0, 0, 16, 16);

        //public Vector2 velocity = new Vector2(0, 0);

        public event OnCollide collideEvent;

        public delegate void OnCollide();

        public HitInfo hits = new HitInfo();

        public string id = "";


        public DynamicCollider()
        {
            CollisionManagement.dynamicColliders.Add(this);
        }

        public DynamicCollider(DynamicRectangle rect)
        {
            dynamicRectangle = rect;
            CollisionManagement.dynamicColliders.Add(this);
        }

    }

    public class TriggerCollider
    {
        public Rectangle bounds = new Rectangle(0, 0, 16, 16);

        public delegate void OnCollide();

        public HitInfo hits = new HitInfo();

        public string id = "";

        public TriggerCollider()
        {
            CollisionManagement.triggerColliders.Add(this);
        }

        public TriggerCollider(Rectangle rect)
        {
            bounds = rect;
            CollisionManagement.triggerColliders.Add(this);
        }

    }

    public class HitInfo
    {
        public List<DynamicCollider> dynamic =  new List<DynamicCollider>();
        public List<Collider> stati =  new List<Collider>();
        public List<TriggerCollider> trigger =  new List<TriggerCollider>();

        public HitInfo()
        {

        }

        public HitInfo(List<DynamicCollider> dynamicColliders)
        {
            dynamic = dynamicColliders;
        }

        public HitInfo(List<Collider> staticColliders)
        {
            stati = staticColliders;
        }

        public HitInfo(List<TriggerCollider> triggerColliders)
        {
            trigger = triggerColliders;
        }
    }

    public class CollisionManagement : Loop
    {

        public static List<Collider> colliders = new List<Collider>();
        public static List<DynamicCollider> dynamicColliders = new List<DynamicCollider>();
        public static List<TriggerCollider> triggerColliders = new List<TriggerCollider>();

        static Vector2 cp, cn = new Vector2();

        static float t;
        // static Rectangle r = new Rectangle(100, 100, 50, 50);
        static bool inter;

        static Vector2 ray_point = new Vector2(200, 20);
        static Vector2 ray_direction = new Vector2(0, 0);

        static DynamicCollider lastDynamic;

        public static bool showHitBoxes = false;

        public override void Update()
        {


            base.Update();
        }

        public static void RemoveStatic(Collider collider)
        {
            colliders.Remove(collider);
        }

        public static void RemoveDynamic(DynamicCollider collider)
        {
            dynamicColliders.Remove(collider);
        }

        public static void DynamicOnDynamic(ref DynamicCollider dynamic)
        {
            List<DynamicCollider> hits = new List<DynamicCollider>();


            for (int i = 0; i < dynamicColliders.Count; i++)
            {
                if (dynamicColliders[i] != dynamic)
                {
                    if (dynamic.dynamicRectangle.ToRectangle().Intersects(dynamicColliders[i].dynamicRectangle.ToRectangle()))
                    {
                        hits.Add(dynamicColliders[i]);//dynamic.dynamicHits
                    }
                }
            }

            dynamic.hits.dynamic = hits;
        }


        public static void TriggerOnStatic(ref TriggerCollider trigger)
        {
            List<Collider> hits = new List<Collider>();

          //  Debug.WriteLine(colliders.Count);


            for (int i = 0; i < colliders.Count; i++)
            {
                if (trigger.bounds.Intersects(colliders[i].bounds))
                {
                    Debug.WriteLine("true");
                    hits.Add(colliders[i]);//dynamic.dynamicHits
                }

            }

            trigger.hits.stati = hits;
        }

        public static void CheckCollision(DynamicCollider dynamic)
        {
            lastDynamic = dynamic;


            colliders = colliders.Distinct().ToList();

            // ray_point = new Vector2(20, 20);
            ray_direction = Input.mousePosition - ray_point;

            //Debug.WriteLine(colliders.Count);


            // dynamic.dynamicRectangle.velocity /*ray_point*/ += GetMovementDirection(Keys.S, Keys.W, Keys.A, Keys.D) /** Time.deltaTime*/;
            // r = AddPosition(r, GetMovementDirection(Keys.Down, Keys.Up, Keys.Left, Keys.Right) /** Time.deltaTime*/);

            //if (AABB.RayVsRect(ref ray_point, ref ray_direction, colliders[1].bounds, ref cp, ref cn, ref t) && t <= 1)
            //    inter = true;
            //else
            //    inter = false;

            // Sort collisions in order of distance
            List<Tuple<int, float>> z = new List<Tuple<int, float>>();

            // Work out collision point, add it to vector along with rect ID
            for (int i = 0; i < colliders.Count; i++)
            {
                if (AABB.DynamicRectVsRect(ref dynamic.dynamicRectangle, Time.deltaTime, ref colliders[i].bounds, ref cp, ref cn, ref t))
                {
                    z.Add(new Tuple<int, float>(i, t));
                }
            }

            z.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            for (int j = 0; j < z.Count; j++)
                AABB.ResolveDynamicRectVsRect(ref dynamic.dynamicRectangle, Time.deltaTime, ref colliders[z[j].Item1].bounds);


            // UPdate the player rectangles position, with its modified velocity
            dynamic.dynamicRectangle.position += dynamic.dynamicRectangle.velocity * Time.gameDelta;
        }

        public static void Draw()
        {
            if (showHitBoxes)
            {
                for (int i = 0; i < colliders.Count; i++)
                    Main.spriteBatch.DrawRectangle(colliders[i].bounds, new Color(255, 255, 255, 0.5f), 2);

                for (int i = 0; i < dynamicColliders.Count; i++)
                {
                    Main.spriteBatch.DrawRectangle(dynamicColliders[i].dynamicRectangle.ToRectangle(), Color.Gray, 2);
                    Main.spriteBatch.DrawLine(dynamicColliders[i].dynamicRectangle.position + dynamicColliders[i].dynamicRectangle.size / 2, dynamicColliders[i].dynamicRectangle.position + dynamicColliders[i].dynamicRectangle.size / 2 + dynamicColliders[i].dynamicRectangle.velocity.NormalizedCopy() * new Vector2(20), Color.Red);
                }

                for (int i = 0; i < triggerColliders.Count; i++)
                    Main.spriteBatch.DrawRectangle(triggerColliders[i].bounds, Color.Red, 2);
            }
        }

    }

    public class AABB
    {
        public static Vector2 Swap(float x, float y)
        {
            float tempswap = x;
            x = y;
            y = tempswap;
            return new Vector2(x, y);
        }

        public static bool ResolveDynamicRectVsRect(ref DynamicRectangle r_dynamic, float fTimeStep, ref Rectangle r_static)
        {

            Vector2 contact_point = new Vector2(), contact_normal = new Vector2();
            float contact_time = 0.0f;
            if (DynamicRectVsRect(ref r_dynamic, fTimeStep, ref r_static, ref contact_point, ref contact_normal, ref contact_time))
            {
                if (contact_normal.Y > 0) r_dynamic.contact[0] = r_static;/* else nullptr;*/
                if (contact_normal.X < 0) r_dynamic.contact[1] = r_static;/* else nullptr;*/
                if (contact_normal.Y < 0) r_dynamic.contact[2] = r_static; /*else nullptr;*/
                if (contact_normal.X > 0) r_dynamic.contact[3] = r_static; /*else nullptr;*/

                r_dynamic.velocity += contact_normal * new Vector2(Math.Abs(r_dynamic.velocity.X), Math.Abs(r_dynamic.velocity.Y)) * (1 - contact_time);
                return true;
            }

            return false;
        }

        public static bool DynamicRectVsRect(ref DynamicRectangle r_dynamic, float fTimeStep, ref Rectangle r_static, ref Vector2 contact_point, ref Vector2 contact_normal, ref float contact_time)
        {
            // Check if dynamic rectangle is actually moving - we assume rectangles are NOT in collision to start
            if (r_dynamic.velocity.X == 0 && r_dynamic.velocity.Y == 0)
                return false;

            // Expand target rectangle by source dimensions
            Rectangle expanded_target = new Rectangle();
            Vector2 expandedTargetPosition = new Vector2(expanded_target.X, expanded_target.Y);

            expanded_target = new Rectangle((int)(r_static.X - r_dynamic.size.X / 2), (int)(r_static.Y - r_dynamic.size.Y / 2), expanded_target.Width, expanded_target.Height); //new Vector2(r_static.X, r_static.Y) - r_dynamic.size / 2;
            expanded_target = new Rectangle(expanded_target.X, expanded_target.Y, (int)(r_static.Width + r_dynamic.size.X), (int)(r_static.Height + r_dynamic.size.Y));//r_static.size + r_dynamic->size;

            Vector2 thing = r_dynamic.position + r_dynamic.size / 2;
            Vector2 thing2 = r_dynamic.velocity * fTimeStep;

            if (RayVsRect(ref thing, ref thing2, expanded_target, ref contact_point, ref contact_normal, ref contact_time))
                return (contact_time >= 0.0f && contact_time < 1.0f);
            else
                return false;
        }

        public static bool RayVsRect(ref Vector2 ray_origin, ref Vector2 ray_dir, Rectangle target, ref Vector2 contact_point, ref Vector2 contact_normal, ref float t_hit_near)
        {
            contact_normal = new Vector2(0, 0);
            contact_point = new Vector2(0, 0);

            // Cache division
            Vector2 invdir = new Vector2(1.0f) / ray_dir;

            Vector2 recPos = new Vector2(target.X, target.Y);
            Vector2 recSize = new Vector2(target.Width, target.Height);

            // Calculate intersections with rectangle bounding axes
            Vector2 t_near = (recPos - ray_origin) * invdir;
            Vector2 t_far = (recPos + recSize - ray_origin) * invdir;

            if (t_far.Y != t_far.Y || t_far.X != t_far.X) return false;
            if (t_near.Y != t_near.Y || t_near.X != t_near.X) return false;


            // Sort distances
            if (t_near.X > t_far.X)
            {
                var the = Swap(t_near.X, t_far.X);

                t_near = new Vector2(the.X, t_near.Y);
                t_far = new Vector2(the.Y, t_far.Y);
            }
            if (t_near.Y > t_far.Y)
            {
                var the = Swap(t_near.Y, t_far.Y);

                t_near = new Vector2(t_near.X, the.X);
                t_far = new Vector2(t_far.X, the.Y);

            }

            // Early rejection		
            if (t_near.X > t_far.Y || t_near.Y > t_far.X) return false;

            // Closest 'time' will be the first contact
            t_hit_near = Math.Max(t_near.X, t_near.Y);

            // Furthest 'time' is contact on opposite side of target
            float t_hit_far = Math.Min(t_far.X, t_far.Y);

            // Reject if ray direction is pointing away from object
            if (t_hit_far < 0)
                return false;

            // Contact point of collision from parametric line equation
            contact_point = ray_origin + t_hit_near * ray_dir;

            if (t_near.X > t_near.Y)
                if (invdir.X < 0)
                    contact_normal = new Vector2(1, 0);
                else
                    contact_normal = new Vector2(-1, 0);

            else if (t_near.X < t_near.Y)
                if (invdir.Y < 0)
                    contact_normal = new Vector2(0, 1);
                else
                    contact_normal = new Vector2(0, -1);

            // Note if t_near == t_far, collision is principly in a diagonal
            // so pointless to resolve. By returning a CN={0,0} even though its
            // considered a hit, the resolver wont change anything.
            return true;
        }
    }

    public class DynamicRectangle
    {
        public Vector2 position = new Vector2();
        public Vector2 size = new Vector2();
        public Vector2 velocity = new Vector2();

        //std::array<olc::aabb::rect*, 4> contact;

        public Rectangle[] contact = new Rectangle[4];

        public DynamicRectangle(Vector2 pos, Vector2 die, Vector2 vel)
        {
            position = pos;
            size = die;
            velocity = vel;
        }

        public DynamicRectangle(Vector2 pos, Vector2 die)
        {
            position = pos;
            size = die;
            velocity = new Vector2();
        }

        public DynamicRectangle(int X, int Y, int Width, int Height, int velX, int velY)
        {
            position = new Vector2(X, Y);
            size = new Vector2(Width, Height);
            velocity = new Vector2(velX, velY);
        }

        public DynamicRectangle(int X, int Y, int Width, int Height)
        {
            position = new Vector2(X, Y);
            size = new Vector2(Width, Height);
        }

        public DynamicRectangle()
        {
            position = new Vector2();
            size = new Vector2();
            velocity = new Vector2();
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }
    }

}
