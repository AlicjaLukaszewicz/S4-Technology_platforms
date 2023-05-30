import org.example.Mage;
import org.example.MageRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.Collection;
import java.util.HashSet;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.AssertionsForClassTypes.assertThatThrownBy;

public class MageRepositoryTest {
    private final String nonexistentName = "Non Existent Name Test";
    private final String existentName = "Existent Name Test";
    private final Mage existentMage = new Mage(existentName, 0);
    private MageRepository mageRepository;

    @BeforeEach
    void initialize() {
        Collection<Mage> collection = new HashSet<>();
        collection.add(existentMage);
        mageRepository = new MageRepository(collection);
    }

    @Test
    public void deleteNonexistentMageThrows() {
        assertThatThrownBy(() -> mageRepository.delete(nonexistentName)).hasMessage("Mage '" + nonexistentName + "' does not exist.");
    }

    @Test
    public void findNonexistentMageReturnsEmptyOptional() {
        assertThat(mageRepository.find(nonexistentName)).isEqualTo(Optional.empty());
    }

    @Test
    public void findExistentMageReturnsNotEmptyOptional() {
        assertThat(mageRepository.find(existentName)).isNotEqualTo(Optional.empty());
    }

    @Test
    public void saveExistentMageThrows() {
        assertThatThrownBy(() -> mageRepository.save(existentMage)).hasMessage("Mage '" + existentMage.getName() + "' does already exist.");
    }
}