#include "movement.h"
#include <unordered_map>

namespace chess {
    static const std::unordered_map<char, char> mirror_lookup_map = {
        { 'a', 'h' },
        { 'b', 'g' },
        { 'c', 'f' },
        { 'd', 'e' },
        { 'e', 'd' },
        { 'f', 'c' },
        { 'g', 'b' },
        { 'h', 'a' },
        { '1', '8' },
        { '2', '7' },
        { '3', '6' },
        { '4', '5' },
        { '5', '4' },
        { '6', '3' },
        { '7', '2' },
        { '8', '1' }
    };

    std::string mirror_position(const char* position) {
        return std::string(1, mirror_lookup_map.at(position[0])) + std::string(1, mirror_lookup_map.at(position[1]));
    }

    std::string move_left(const char* position) {
        if (position[0] == 'a')
            return "";
        return std::string(1, position[0] - 1) + position[1];
    }

    std::string move_right(const char* position) {
        if (position[0] == 'h')
            return "";
        return std::string(1, position[0] + 1) + position[1];
    }

    std::string move_up(const char* position) {
        if (position[1] == '8')
            return "";
        return position[0] + std::string(1, position[1] + 1);
    }

    std::string move_down(const char* position) {
        if (position[1] == '1')
            return "";
        return position[0] + std::string(1, position[1] - 1);
    }

    std::string move_upper_left(const char* position) {
        if (position[0] == 'a')
            return "";
        if (position[1] == '8')
            return "";
        return std::string(1, position[0] - 1) + std::string(1, position[1] + 1);
    }

    std::string move_upper_right(const char* position) {
        if (position[0] == 'h')
            return "";
        if (position[1] == '8')
            return "";
        return std::string(1, position[0] + 1) + std::string(1, position[1] + 1);
    }

    std::string move_lower_left(const char* position) {
        if (position[0] == 'a')
            return "";
        if (position[1] == '1')
            return "";
        return std::string(1, position[0] - 1) + std::string(1, position[1] - 1);
    }

    std::string move_lower_right(const char* position) {
        if (position[0] == 'h')
            return "";
        if (position[1] == '1')
            return "";
        return std::string(1, position[0] + 1) + std::string(1, position[1] - 1);
    }

    std::string move_direction(std::shared_ptr<board> board, direction dir, const char* position, uint8_t& collision) {
        std::string pos;

        switch (dir) {
            case up:
                pos = move_up(position);
                break;
            case right:
                pos = move_right(position);
                break;
            case left:
                pos = move_left(position);
                break;
            case down:
                pos = move_down(position);
                break;
            case upper_left:
                pos = move_upper_left(position);
                break;
            case upper_right:
                pos = move_upper_right(position);
                break;
            case lower_left:
                pos = move_lower_left(position);
                break;
            case lower_right:
                pos = move_lower_right(position);
                break;
            default:
                return "";
        }

        if (pos.empty())
            return "";
        auto value = board->get(pos.c_str());

        if (value)
            collision = value;
        return pos;
    }

    std::vector<std::string> moves_till_end(std::shared_ptr<board> board, direction dir, const char* position, uint8_t& collision) {
        std::vector<std::string> result;
        auto string = move_direction(board, dir, position, collision);
        result.reserve(7);

        while (!string.empty()) {
            result.push_back(string);
            if (collision)
                break;
            string = move_direction(board, dir, string.c_str(), collision);
        }

        return result;
    }

    std::string move_knight(std::shared_ptr<board> board, direction first, direction second, const char* position, uint8_t& collision) {
        switch (first) {
            case upper_left: {
                auto upper_left = move_upper_left(position);
                if (upper_left.empty())
                    return "";

                switch (second) {
                    case left: {
                        auto pos = move_left(upper_left.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    case up: {
                        auto pos = move_up(upper_left.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    default:
                        return "";
                }
            }
            case upper_right: {
                auto upper_right = move_upper_right(position);
                if (upper_right.empty())
                    return "";

                switch (second) {
                    case right: {
                        auto pos = move_right(upper_right.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    case up: {
                        auto pos = move_up(upper_right.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    default:
                        return "";
                }
            }
            case lower_left: {
                auto lower_left = move_lower_left(position);
                if (lower_left.empty())
                    return "";

                switch (second) {
                    case left: {
                        auto pos = move_left(lower_left.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    case down: {
                        auto pos = move_down(lower_left.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    default:
                        return "";
                }
            }
            case lower_right: {
                auto lower_right = move_lower_right(position);
                if (lower_right.empty())
                    return "";

                switch (second) {
                    case right: {
                        auto pos = move_right(lower_right.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    case down: {
                        auto pos = move_down(lower_right.c_str());
                        if (pos.empty())
                            return "";
                        auto value = board->get(pos.c_str());

                        if (value)
                            collision = value;
                        return pos;
                    }
                    default:
                        return "";
                }
            }
            default:
                return "";
        }
    }
}
