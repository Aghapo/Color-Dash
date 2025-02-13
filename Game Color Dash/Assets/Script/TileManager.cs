using UnityEngine;

public class TileManager : MonoBehaviour {
    public GameObject[] tilePrefabs; // D�z, d�n�� ve �atall� ray tile'lar�
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public float tileSize = 5f; // Ray�n boyutu (her tile aras� 5 birim olursa)

    private GameObject[,] gridTiles; // Grid�de tutulan raylar

    void Start() {
        gridTiles = new GameObject[gridSizeX, gridSizeY];

        // �lk ray� ortada ba�lat
        PlaceTile(5, 5, 0);
    }

    void PlaceTile(int x, int y, int prefabIndex) {
        if (gridTiles[x, y] != null) return; // E�er burada zaten ray varsa ekleme

        Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);
        GameObject tile = Instantiate(tilePrefabs[prefabIndex], position, Quaternion.identity);
        gridTiles[x, y] = tile; // Grid'e ekle
    }
}
