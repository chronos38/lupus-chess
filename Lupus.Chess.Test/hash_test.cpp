#include <gtest/gtest.h>
#include <chess/hash.h>
#include <chess/board.h>

using namespace chess;

class hash_test : public ::testing::Test
{
public:
    virtual void SetUp() override {
        zobrist_initialize();
    }

    virtual void TearDown() override {
    }
};

TEST_F(hash_test, check_if_valid) {
    auto b = board::create_starting_board();
    auto h = zobrist_hash(b);
    ASSERT_NE(0, h);
}
