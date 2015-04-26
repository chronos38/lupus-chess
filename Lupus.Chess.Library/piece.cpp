#include "piece.h"
#include "board.h"
#include "move.h"
#include "movement.h"
#include <future>

namespace chess {
    static const int end_game_transition_count = 11;
    static const std::unordered_map<piece_type, int> attack_score_map = {
        { king, 2000 },
        { queen, 100 },
        { knight, 35 },
        { bishop, 35 },
        { rook, 50 },
        { pawn, 10 }
    };
    static const std::unordered_map<piece_type, int> defense_score_map = {
        { king, 0 },
        { queen, 100 },
        { knight, 35 },
        { bishop, 35 },
        { rook, 50 },
        { pawn, 10 }
    };
    static const array_2d<int, 8, 8> center_position_score_array = {
        -15, -15, -15, -15, -15, -15, -15, -15,
        -15, 0, 0, 0, 0, 0, 0, -15,
        -15, 0, 10, 10, 10, 10, 0, -15,
        -15, 0, 10, 25, 25, 10, 0, -15,
        -15, 0, 10, 25, 25, 10, 0, -15,
        -15, 0, 10, 10, 10, 10, 0, -15,
        -15, 0, 0, 0, 0, 0, 0, -15,
        -15, -15, -15, -15, -15, -15, -15, -15
    };
    static const array_2d<int, 8, 8> king_position_score_array = {
        55, 45, 25, 10, 10, 25, 45, 55,
        10, 5, 0, -5, -5, 0, 5, 10,
        -10, -10, -10, -15, -15 -10, -10, -10,
        -25, -25, -25, -35, -35, -25, -25, -25,
        -35, -35, -35, -45, -45, -35, -35, -35,
        -50, -50, -50, -50, -50, -50, -50, -50,
        -100, -100, -100, -100, -100, -100, -100, -100,
        -200, -200, -200, -200, -200, -200, -200, -200
    };
    static const array_2d<int, 8, 8> pawn_position_score_array = {
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 7, 10, 10, 7, 5, 5,
        10, 10, 10, 15, 15, 10, 10, 10,
        15, 15, 15, 15, 15, 15, 15, 15,
        20, 20, 20, 20, 20, 20, 20, 20,
        100, 100, 100, 100, 100, 100, 100, 100
    };

    const char* piece_position(std::shared_ptr<board> on_board, piece_value for_value, std::array<char, 3>& position) {
        auto file = 0;
        auto rank = 1;

        for (auto&& square : *on_board) {
            if (square == for_value) {
                position[0] = file + 'a';
                position[1] = rank + '0';
                return position.data();
            }

            file = (file + 1) % 8;
            if (file == 0)
                rank += 1;
        }

        return "";
    }

    class piece_queen : public piece_state {
    public:
        explicit piece_queen(const char* position, piece_color color) : piece_state(position, color) {
            score_ = 1000;
        }

        virtual ~piece_queen() = default;
        
        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_queen>(position_.data(), color_);
        }

        virtual void update(ipiece* piece) override {
            std::future<std::vector<std::string>> tasks[8];
            auto index = 0;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            tasks[0] = async(std::launch::async, moves_till_end, piece->board(), left, position_.data(), std::ref(collisions_[0]));
            tasks[1] = async(std::launch::async, moves_till_end, piece->board(), right, position_.data(), std::ref(collisions_[1]));
            tasks[2] = async(std::launch::async, moves_till_end, piece->board(), up, position_.data(), std::ref(collisions_[2]));
            tasks[3] = async(std::launch::async, moves_till_end, piece->board(), down, position_.data(), std::ref(collisions_[3]));
            tasks[4] = async(std::launch::async, moves_till_end, piece->board(), lower_left, position_.data(), std::ref(collisions_[4]));
            tasks[5] = async(std::launch::async, moves_till_end, piece->board(), lower_right, position_.data(), std::ref(collisions_[5]));
            tasks[6] = async(std::launch::async, moves_till_end, piece->board(), upper_left, position_.data(), std::ref(collisions_[6]));
            tasks[7] = async(std::launch::async, moves_till_end, piece->board(), upper_right, position_.data(), std::ref(collisions_[7]));
            auto row = position_[1] - '1';
            auto column = position_[0] - 'a';
            position_score_ = center_position_score_array.get(row, column);
            moves_.clear();

            for (auto&& task : tasks) {
                auto positions = task.get();
                auto collision = collisions_[index++];
                if (positions.empty())
                    continue;
                
                for (auto&& position : positions) {
                    moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                }

                if (!collision)
                    continue;
                auto collision_color = value_to_color(static_cast<piece_value>(collision));
                auto collision_type = value_to_type(static_cast<piece_value>(collision));

                if (collision_color == color_) {
                    defense_score_ += defense_score_map.at(collision_type);
                    moves_.pop_back();
                } else {
                    attack_score_ += attack_score_map.at(collision_type);
                }
            }
        }
        
        virtual piece_value value() const override {
            switch (color_) {
                case white:
                    return white_queen;
                case black:
                    return black_queen;
                default:
                    throw std::domain_error("could not determine piece value");
            }
        }
        
        virtual piece_type type() const override {
            return queen;
        }
    };

    class piece_rook : public piece_state {
    public:
        explicit piece_rook(const char* position, piece_color color) : piece_state(position, color) {
            score_ = 525;
        }

        virtual ~piece_rook() = default;

        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_rook>(position_.data(), color_);
        }

        virtual void update(ipiece* piece) override {
            std::future<std::vector<std::string>> tasks[4];
            auto index = 0;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            tasks[0] = async(std::launch::async, moves_till_end, piece->board(), left, position_.data(), std::ref(collisions_[0]));
            tasks[1] = async(std::launch::async, moves_till_end, piece->board(), right, position_.data(), std::ref(collisions_[1]));
            tasks[2] = async(std::launch::async, moves_till_end, piece->board(), up, position_.data(), std::ref(collisions_[2]));
            tasks[3] = async(std::launch::async, moves_till_end, piece->board(), down, position_.data(), std::ref(collisions_[3]));
            auto row = position_[1] - '1';
            auto column = position_[0] - 'a';
            position_score_ = center_position_score_array.get(row, column);
            moves_.clear();

            for (auto&& task : tasks) {
                auto positions = task.get();
                auto collision = collisions_[index++];
                if (positions.empty())
                    continue;

                for (auto&& position : positions) {
                    moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                }

                if (!collision)
                    continue;
                auto collision_color = value_to_color(static_cast<piece_value>(collision));
                auto collision_type = value_to_type(static_cast<piece_value>(collision));

                if (collision_color == color_) {
                    defense_score_ += defense_score_map.at(collision_type);
                    moves_.pop_back();
                } else {
                    attack_score_ += attack_score_map.at(collision_type);
                }
            }
        }

        virtual piece_value value() const override {
            switch (color_) {
                case white:
                    return white_rook;
                case black:
                    return black_rook;
                default:
                    throw std::domain_error("could not determine piece value");
            }
        }
        
        virtual piece_type type() const override {
            return rook;
        }
    };

    class piece_bishop : public piece_state {
    public:
        explicit piece_bishop(const char* position, piece_color color) : piece_state(position, color) {
            score_ = 350;
        }

        virtual ~piece_bishop() = default;

        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_bishop>(position_.data(), color_);
        }
        
        virtual void update(ipiece* piece) override {
            std::future<std::vector<std::string>> tasks[4];
            auto index = 0;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            tasks[0] = async(std::launch::async, moves_till_end, piece->board(), lower_left, position_.data(), std::ref(collisions_[0]));
            tasks[1] = async(std::launch::async, moves_till_end, piece->board(), lower_right, position_.data(), std::ref(collisions_[1]));
            tasks[2] = async(std::launch::async, moves_till_end, piece->board(), upper_left, position_.data(), std::ref(collisions_[2]));
            tasks[3] = async(std::launch::async, moves_till_end, piece->board(), upper_right, position_.data(), std::ref(collisions_[3]));
            auto row = position_[1] - '1';
            auto column = position_[0] - 'a';
            position_score_ = center_position_score_array.get(row, column);
            moves_.clear();

            for (auto&& task : tasks) {
                auto positions = task.get();
                auto collision = collisions_[index++];
                if (positions.empty())
                    continue;

                for (auto&& position : positions) {
                    moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                }

                if (!collision)
                    continue;
                auto collision_color = value_to_color(static_cast<piece_value>(collision));
                auto collision_type = value_to_type(static_cast<piece_value>(collision));

                if (collision_color == color_) {
                    defense_score_ += defense_score_map.at(collision_type);
                    moves_.pop_back();
                } else {
                    attack_score_ += attack_score_map.at(collision_type);
                }
            }
        }

        virtual piece_value value() const override {
            switch (color_) {
                case white:
                    return white_bishop;
                case black:
                    return black_bishop;
                default:
                    throw std::domain_error("could not determine piece value");
            }
        }
        
        virtual piece_type type() const override {
            return bishop;
        }
    };

    class piece_knight : public piece_state {
    public:
        explicit piece_knight(const char* position, piece_color color) : piece_state(position, color) {
            score_ = 350;
        }

        virtual ~piece_knight() = default;
        
        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_knight>(position_.data(), color_);
        }

        
        virtual void update(ipiece* piece) override {
            std::future<std::string> tasks[8];
            auto index = 0;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            tasks[0] = async(std::launch::async, move_knight, piece->board(), lower_left, left, position_.data(), std::ref(collisions_[0]));
            tasks[1] = async(std::launch::async, move_knight, piece->board(), lower_left, down, position_.data(), std::ref(collisions_[1]));
            tasks[2] = async(std::launch::async, move_knight, piece->board(), lower_right, right, position_.data(), std::ref(collisions_[2]));
            tasks[3] = async(std::launch::async, move_knight, piece->board(), lower_right, down, position_.data(), std::ref(collisions_[3]));
            tasks[4] = async(std::launch::async, move_knight, piece->board(), upper_left, left, position_.data(), std::ref(collisions_[4]));
            tasks[5] = async(std::launch::async, move_knight, piece->board(), upper_left, up, position_.data(), std::ref(collisions_[5]));
            tasks[6] = async(std::launch::async, move_knight, piece->board(), upper_right, right, position_.data(), std::ref(collisions_[6]));
            tasks[7] = async(std::launch::async, move_knight, piece->board(), upper_right, up, position_.data(), std::ref(collisions_[7]));
            auto row = position_[1] - '1';
            auto column = position_[0] - 'a';
            position_score_ = center_position_score_array.get(row, column);
            moves_.clear();

            for (auto&& task : tasks) {
                auto position = task.get();
                auto collision = collisions_[index++];
                if (position.empty())
                    continue;
                moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));

                if (!collision)
                    continue;
                auto collision_color = value_to_color(static_cast<piece_value>(collision));
                auto collision_type = value_to_type(static_cast<piece_value>(collision));

                if (collision_color == color_) {
                    defense_score_ += defense_score_map.at(collision_type);
                    moves_.pop_back();
                } else {
                    attack_score_ += attack_score_map.at(collision_type);
                }
            }
        }

        virtual piece_value value() const override {
            switch (color_) {
                case white:
                    return white_knight;
                case black:
                    return black_knight;
                default:
                    throw std::domain_error("could not determine piece value");
            }
        }

        virtual piece_type type() const override {
            return knight;
        }
    };

    class piece_pawn : public piece_state {
    public:
        explicit piece_pawn(const char* position, piece_color color) : piece_state(position, color) {
            score_ = 100;
        }

        virtual ~piece_pawn() = default;

        virtual piece_value value() const override {
            switch (color_) {
                case white:
                    return white_pawn;
                case black:
                    return black_pawn;
                default:
                    throw std::domain_error("could not determine piece value");
            }
        }

        virtual piece_type type() const override {
            return pawn;
        }
    };

    class piece_pawn_white : public piece_pawn {
    public:
        explicit piece_pawn_white(const char* position) : piece_pawn(position, white) {
        }

        virtual ~piece_pawn_white() = default;
        
        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_pawn_white>(position_.data());
        }

        virtual void update(ipiece* piece) override {
            std::future<std::string> tasks[2];
            auto index = 1;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            auto task_up = async(std::launch::async, move_direction, piece->board(), up, position_.data(), std::ref(collisions_[0]));
            tasks[0] = async(std::launch::async, move_direction, piece->board(), upper_left, position_.data(), std::ref(collisions_[1]));
            tasks[1] = async(std::launch::async, move_direction, piece->board(), upper_right, position_.data(), std::ref(collisions_[2]));
            auto row = position_[1] - '1';
            auto column = position_[0] - 'a';
            position_score_ = pawn_position_score_array.get(row, column);
            moves_.clear();

            auto position = task_up.get();
            if (!position.empty() && !collisions_[0]) {
                moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));

                if (position_[1] == '2') {
                    uint8_t collision = 0;
                    auto double_pos = std::string(1, position_[0]) + '3';
                    position = move_direction(piece->board(), up, double_pos.c_str(), collision);

                    if (!collision && !position.empty())
                        moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                }
            } else {
                position_score_ -= 10;
            }

            for (auto&& task : tasks) {
                position = task.get();
                auto collision = collisions_[index++];
                if (position.empty())
                    continue;
                if (collision) {
                    auto collision_color = value_to_color(static_cast<piece_value>(collision));
                    auto collision_type = value_to_type(static_cast<piece_value>(collision));

                    if (collision_color == color_) {
                        defense_score_ += defense_score_map.at(collision_type);
                    } else {
                        attack_score_ += attack_score_map.at(collision_type);
                        moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                    }
                } else if (position == piece->board()->en_passant()) {
                    moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                }
            }
        }
    };

    class piece_pawn_black : public piece_pawn {
    public:
        explicit piece_pawn_black(const char* position) : piece_pawn(position, black) {
        }

        virtual ~piece_pawn_black() = default;
        
        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_pawn_black>(position_.data());
        }

        virtual void update(ipiece* piece) override {
            std::future<std::string> tasks[2];
            auto index = 1;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            auto task_up = async(std::launch::async, move_direction, piece->board(), down, position_.data(), std::ref(collisions_[0]));
            tasks[0] = async(std::launch::async, move_direction, piece->board(), lower_left, position_.data(), std::ref(collisions_[1]));
            tasks[1] = async(std::launch::async, move_direction, piece->board(), lower_right, position_.data(), std::ref(collisions_[2]));
            auto mirror = mirror_position(position_.data());
            auto row = mirror[1] - '0';
            auto column = mirror[0] - 'a';
            position_score_ = pawn_position_score_array.get(row, column);
            moves_.clear();

            auto position = task_up.get();
            if (!position.empty() && !collisions_[0]) {
                moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));

                if (position_[1] == '7') {
                    uint8_t collision = 0;
                    auto double_pos = std::string(1, position_[0]) + '6';
                    position = move_direction(piece->board(), down, double_pos.c_str(), collision);

                    if (!collision && !position.empty())
                        moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                }
            } else {
                position_score_ -= 10;
            }

            for (auto&& task : tasks) {
                position = task.get();
                auto collision = collisions_[index++];
                if (position.empty())
                    continue;
                if (collision) {
                    auto collision_color = value_to_color(static_cast<piece_value>(collision));
                    auto collision_type = value_to_type(static_cast<piece_value>(collision));

                    if (collision_color == color_) {
                        defense_score_ += defense_score_map.at(collision_type);
                    } else {
                        attack_score_ += attack_score_map.at(collision_type);
                        moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                    }
                } else if (position == piece->board()->en_passant()) {
                    moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));
                }
            }
        }
    };

    class piece_king : public piece_state {
    public:
        explicit piece_king(const char* position, piece_color color) : piece_state(position, color) {
            score_ = 20000;
        }

        virtual ~piece_king() = default;

        virtual piece_value value() const override {
            switch (color_) {
                case white:
                    return white_king;
                case black:
                    return black_king;
                default:
                    throw std::domain_error("could not determine piece value");
            }
        }

        virtual piece_type type() const override {
            return king;
        }
    };

    class piece_king_end_game : public piece_king {
    public:
        explicit piece_king_end_game(const char* position, piece_color color) : piece_king(position, color) {}

        virtual ~piece_king_end_game() = default;

        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_king_end_game>(position_.data(), color_);
        }

        virtual void update(ipiece* piece) override {
            std::future<std::string> tasks[8];
            auto index = 0;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            tasks[0] = async(std::launch::async, move_direction, piece->board(), left, position_.data(), std::ref(collisions_[0]));
            tasks[1] = async(std::launch::async, move_direction, piece->board(), right, position_.data(), std::ref(collisions_[1]));
            tasks[2] = async(std::launch::async, move_direction, piece->board(), up, position_.data(), std::ref(collisions_[2]));
            tasks[3] = async(std::launch::async, move_direction, piece->board(), down, position_.data(), std::ref(collisions_[3]));
            tasks[4] = async(std::launch::async, move_direction, piece->board(), lower_left, position_.data(), std::ref(collisions_[4]));
            tasks[5] = async(std::launch::async, move_direction, piece->board(), lower_right, position_.data(), std::ref(collisions_[5]));
            tasks[6] = async(std::launch::async, move_direction, piece->board(), upper_left, position_.data(), std::ref(collisions_[6]));
            tasks[7] = async(std::launch::async, move_direction, piece->board(), upper_right, position_.data(), std::ref(collisions_[7]));
            auto row = position_[1] - '1';
            auto column = position_[0] - 'a';
            position_score_ = center_position_score_array.get(row, column);
            moves_.clear();

            for (auto&& task : tasks) {
                auto position = task.get();
                auto collision = collisions_[index++];
                if (position.empty())
                    continue;
                moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));

                if (!collision)
                    continue;
                auto collision_color = value_to_color(static_cast<piece_value>(collision));
                auto collision_type = value_to_type(static_cast<piece_value>(collision));

                if (collision_color == color_) {
                    defense_score_ += defense_score_map.at(collision_type);
                    moves_.pop_back();
                } else {
                    attack_score_ += attack_score_map.at(collision_type);
                }
            }
        }
    };

    class piece_king_white : public piece_king {
    public:
        piece_king_white(const char* position) : piece_king(position, white) {
        }

        virtual ~piece_king_white() = default;

        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_king_white>(position_.data());
        }
        
        virtual void update(ipiece* piece) override {
            if (piece->board()->count() <= end_game_transition_count) {
                change_state(piece, std::make_unique<piece_king_end_game>(position_.data(), color_));
                piece->update();
                return;
            }

            std::future<std::string> tasks[8];
            auto index = 0;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            tasks[0] = async(std::launch::async, move_direction, piece->board(), left, position_.data(), std::ref(collisions_[0]));
            tasks[1] = async(std::launch::async, move_direction, piece->board(), right, position_.data(), std::ref(collisions_[1]));
            tasks[2] = async(std::launch::async, move_direction, piece->board(), up, position_.data(), std::ref(collisions_[2]));
            tasks[3] = async(std::launch::async, move_direction, piece->board(), down, position_.data(), std::ref(collisions_[3]));
            tasks[4] = async(std::launch::async, move_direction, piece->board(), lower_left, position_.data(), std::ref(collisions_[4]));
            tasks[5] = async(std::launch::async, move_direction, piece->board(), lower_right, position_.data(), std::ref(collisions_[5]));
            tasks[6] = async(std::launch::async, move_direction, piece->board(), upper_left, position_.data(), std::ref(collisions_[6]));
            tasks[7] = async(std::launch::async, move_direction, piece->board(), upper_right, position_.data(), std::ref(collisions_[7]));
            auto row = position_[1] - '1';
            auto column = position_[0] - 'a';
            position_score_ = king_position_score_array.get(row, column);
            moves_.clear();

            for (auto&& task : tasks) {
                auto position = task.get();
                auto collision = collisions_[index++];
                if (position.empty())
                    continue;
                moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));

                if (!collision)
                    continue;
                auto collision_color = value_to_color(static_cast<piece_value>(collision));
                auto collision_type = value_to_type(static_cast<piece_value>(collision));

                if (collision_color == color_) {
                    defense_score_ += defense_score_map.at(collision_type);
                    moves_.pop_back();
                } else {
                    attack_score_ += attack_score_map.at(collision_type);
                }
            }
        }
    };

    class piece_king_black : public piece_king {
    public:
        piece_king_black(const char* position) : piece_king(position, black) {
        }

        virtual ~piece_king_black() = default;

        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_king_black>(position_.data());
        }
        
        virtual void update(ipiece* piece) override {
            if (piece->board()->count() <= end_game_transition_count) {
                change_state(piece, std::make_unique<piece_king_end_game>(position_.data(), color_));
                piece->update();
                return;
            }

            std::future<std::string> tasks[8];
            auto index = 0;
            collisions_.fill(0);
            attack_score_ = 0;
            defense_score_ = 0;

            tasks[0] = async(std::launch::async, move_direction, piece->board(), left, position_.data(), std::ref(collisions_[0]));
            tasks[1] = async(std::launch::async, move_direction, piece->board(), right, position_.data(), std::ref(collisions_[1]));
            tasks[2] = async(std::launch::async, move_direction, piece->board(), up, position_.data(), std::ref(collisions_[2]));
            tasks[3] = async(std::launch::async, move_direction, piece->board(), down, position_.data(), std::ref(collisions_[3]));
            tasks[4] = async(std::launch::async, move_direction, piece->board(), lower_left, position_.data(), std::ref(collisions_[4]));
            tasks[5] = async(std::launch::async, move_direction, piece->board(), lower_right, position_.data(), std::ref(collisions_[5]));
            tasks[6] = async(std::launch::async, move_direction, piece->board(), upper_left, position_.data(), std::ref(collisions_[6]));
            tasks[7] = async(std::launch::async, move_direction, piece->board(), upper_right, position_.data(), std::ref(collisions_[7]));
            auto mirror = mirror_position(position_.data());
            auto row = mirror[1] - '0';
            auto column = mirror[0] - 'a';
            position_score_ = king_position_score_array.get(row, column);
            moves_.clear();

            for (auto&& task : tasks) {
                auto position = task.get();
                auto collision = collisions_[index++];
                if (position.empty())
                    continue;
                moves_.emplace_back(std::make_shared<move>(piece, position_.data(), position.c_str()));

                if (!collision)
                    continue;
                auto collision_color = value_to_color(static_cast<piece_value>(collision));
                auto collision_type = value_to_type(static_cast<piece_value>(collision));

                if (collision_color == color_) {
                    defense_score_ += defense_score_map.at(collision_type);
                    moves_.pop_back();
                } else {
                    attack_score_ += attack_score_map.at(collision_type);
                }
            }
        }
    };

    piece::piece(std::shared_ptr<chess::board> board, const char* position) : board_(board) {
        auto square = board->get(position);

        if (!square)
            throw std::invalid_argument("position points to an empty square");
        auto value = static_cast<piece_value>(square);
        auto color = value_to_color(value);
        auto type = value_to_type(value);

        switch (type) {
            case pawn:
                switch (color) {
                    case white:
                        state_ = std::make_unique<piece_pawn_white>(position);
                        break;
                    case black:
                        state_ = std::make_unique<piece_pawn_black>(position);
                        break;
                }
                break;
            case knight:
                state_ = std::make_unique<piece_knight>(position, color);
                break;
            case bishop:
                state_ = std::make_unique<piece_bishop>(position, color);
                break;
            case rook:
                state_ = std::make_unique<piece_rook>(position, color);
                break;
            case queen:
                state_ = std::make_unique<piece_queen>(position, color);
                break;
            case king:
                if (board->count() <= end_game_transition_count) {
                    state_ = std::make_unique<piece_king_end_game>(position, color);
                } else {
                    switch (color) {
                        case white:
                            state_ = std::make_unique<piece_king_white>(position);
                            break;
                        case black:
                            state_ = std::make_unique<piece_king_black>(position);
                            break;
                    }
                }
                break;
        }
    }

    piece::piece(piece&& other) {
        swap(board_, other.board_);
        swap(state_, other.state_);
    }

    piece::piece(const piece& other) {
        board_ = other.board_;
        state_ = other.state_->clone();
    }

    void piece::update() {
        state_->update(this);
    }

    int piece::score() const {
        return state_->score();
    }

    int piece::attack_score() const {
        return state_->attack_score();
    }

    int piece::defense_score() const {
        return state_->defense_score();
    }

    int piece::position_score() const {
        return state_->position_score();
    }

    std::vector<std::shared_ptr<move>> piece::allowed_moves() const {
        return state_->allowed_moves();
    }

    piece_value piece::value() const {
        return state_->value();
    }

    piece_type piece::type() const {
        return state_->type();
    }

    piece_color piece::color() const {
        return state_->color();
    }

    const char* piece::position() const {
        return state_->position();
    }

    void piece::set_position(const char* value) {
        state_->set_position(value);
    }

    std::shared_ptr<board> piece::board() const {
        return board_;
    }

    piece& piece::operator=(piece&& other) {
        swap(board_, other.board_);
        swap(state_, other.state_);
        other.board_ = nullptr;
        other.state_ = nullptr;
        return *this;
    }

    piece& piece::operator=(const piece& other) {
        board_ = other.board_;
        state_ = other.state_->clone();
        return *this;
    }

    piece_state::piece_state(const char* position, piece_color color) : color_(color) {
        position_[0] = position[0];
        position_[1] = position[1];
        position_[2] = 0;
    }

    int piece_state::score() const {
        return score_;
    }

    int piece_state::attack_score() const {
        return attack_score_;
    }

    int piece_state::defense_score() const {
        return defense_score_;
    }

    int piece_state::position_score() const {
        return position_score_;
    }

    std::vector<std::shared_ptr<move>> piece_state::allowed_moves() const {
        return moves_;
    }

    const char* piece_state::position() const {
        return position_.data();
    }

    void piece_state::set_position(const char* value) {
        position_[0] = value[0];
        position_[1] = value[1];
        position_[2] = 0;
    }

    piece_color piece_state::color() const {
        return color_;
    }

    void piece_state::change_state(ipiece* piece, std::unique_ptr<piece_state> state) {
        piece->state_ = std::move(state);
    }
}
