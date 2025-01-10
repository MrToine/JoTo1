using Godot;
using System.Collections.Generic;

public class TrailMesh : MeshInstance3D
{
	[Export]
	public int MaxTrailPoints = 20; // Nombre maximum de points dans la traînée.

	private List<Vector3> trailPoints = new List<Vector3>();
	private Mesh trailMesh;
	private Vector3 lastHeaderPosition;

	private Node3D boltHeader;

	public override void _Ready()
	{
		// Récupérer le "BoltHeader" pour le suivre.
		boltHeader = GetParent().GetNode<Node3D>("BoltHeader");
		lastHeaderPosition = boltHeader.GlobalTransform.origin;

		// Créer un nouveau mesh pour la traînée.
		trailMesh = new QuadMesh();
		((QuadMesh)trailMesh).Size = new Vector2(0.5f, 0.1f); // Largeur de la traînée.
		Mesh = trailMesh;
	}

	public override void _Process(double delta)
	{
		// Ajouter la position actuelle du BoltHeader à la liste de points.
		Vector3 currentHeaderPosition = boltHeader.GlobalTransform.origin;

		// Ajouter un nouveau point si le BoltHeader s'est déplacé.
		if ((currentHeaderPosition - lastHeaderPosition).Length() > 0.1f)
		{
			trailPoints.Insert(0, currentHeaderPosition);
			lastHeaderPosition = currentHeaderPosition;
		}

		// Limiter la taille de la liste.
		if (trailPoints.Count > MaxTrailPoints)
		{
			trailPoints.RemoveAt(trailPoints.Count - 1);
		}

		// Mettre à jour le mesh pour refléter les nouveaux points.
		UpdateTrailMesh();
	}

	private void UpdateTrailMesh()
	{
		if (trailPoints.Count < 2)
			return;

		// Créer un nouveau ArrayMesh.
		ArrayMesh arrayMesh = new ArrayMesh();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Color> colors = new List<Color>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> indices = new List<int>();

		for (int i = 0; i < trailPoints.Count - 1; i++)
		{
			Vector3 current = trailPoints[i];
			Vector3 next = trailPoints[i + 1];
			Vector3 direction = (next - current).Normalized();
			Vector3 cross = direction.Cross(Vector3.Up) * 0.5f;

			// Ajoute des sommets pour les bords gauche et droit.
			vertices.Add(current + cross); // Gauche
			vertices.Add(current - cross); // Droite

			// Ajoute des normales et des UVs.
			normals.Add(Vector3.Up);
			normals.Add(Vector3.Up);

			uvs.Add(new Vector2(0, i / (float)trailPoints.Count));
			uvs.Add(new Vector2(1, i / (float)trailPoints.Count));

			// Ajoute des couleurs (optionnel, pour des effets visuels).
			float alpha = 1.0f - (i / (float)trailPoints.Count);
			colors.Add(new Color(1, 1, 1, alpha));
			colors.Add(new Color(1, 1, 1, alpha));

			// Ajoute des indices pour former les triangles.
			if (i < trailPoints.Count - 2)
			{
				int index = i * 2;
				indices.Add(index);
				indices.Add(index + 1);
				indices.Add(index + 2);

				indices.Add(index + 1);
				indices.Add(index + 3);
				indices.Add(index + 2);
			}
		}

		// Mettre à jour le ArrayMesh.
		Godot.Collections.Array meshData = new Godot.Collections.Array();
		meshData.Resize((int)ArrayMesh.ArrayType.Max);
		meshData[(int)ArrayMesh.ArrayType.Vertex] = vertices.ToArray();
		meshData[(int)ArrayMesh.ArrayType.Normal] = normals.ToArray();
		meshData[(int)ArrayMesh.ArrayType.Color] = colors.ToArray();
		meshData[(int)ArrayMesh.ArrayType.TexUv] = uvs.ToArray();
		meshData[(int)ArrayMesh.ArrayType.Index] = indices.ToArray();

		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, meshData);
		Mesh = arrayMesh;
	}
}
