import org.example.DTO;
import org.example.Mage;
import org.example.MageController;
import org.example.MageRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.Optional;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;
import static org.mockito.Mockito.*;

public class MageControllerTest {
    private final String nonexistentName = "Non Existing Name Test";
    private final String existentName = "Existent Name Test";
    private final Mage existentMage = new Mage(existentName, 0);
    private MageRepository mageRepository;
    private MageController mageController;

    @BeforeEach
    public void initialize() {
        mageRepository = mock(MageRepository.class);
        mageController = new MageController(mageRepository);
    }

    @Test
    public void deleteExistentMageReturnsDone() {
        assertThat(mageController.delete(existentName)).isEqualTo("done");
    }

    @Test
    public void deleteNonexistentMageReturnsNotFound() {
        doThrow(new IllegalArgumentException("Mage '" + nonexistentName + "' does not exist.")).when(mageRepository).delete(nonexistentName);
        assertThat(mageController.delete(nonexistentName)).isEqualTo("not found");
    }

    @Test
    public void findNonexistentMageReturnsNotFound() {
        doReturn(Optional.empty()).when(mageRepository).find(nonexistentName);
        assertThat(mageController.find(nonexistentName)).isEqualTo("not found");
    }

    @Test
    public void findReturnsFoundMageString() {
        doReturn(Optional.of(existentMage)).when(mageRepository).find(existentName);
        assertThat(mageController.find(existentName)).isEqualTo(existentMage.toString());
    }

    @Test
    public void saveNonexistentMageReturnsDone() {
        DTO dto = new DTO();
        dto.name = existentName;
        dto.level = 10;
        assertThat(mageController.save(dto)).isEqualTo("done");
    }

    @Test
    public void saveExistentMageReturnsBadRequest() {
        doThrow(new IllegalArgumentException("Mage '" + existentMage.getName() + "' does already exist.")).when(mageRepository).save(any(Mage.class));
        DTO dto = new DTO();
        dto.name = existentMage.getName();
        dto.level = existentMage.getLevel();
        assertThat(mageController.save(dto)).isEqualTo("bad request");
    }
}
